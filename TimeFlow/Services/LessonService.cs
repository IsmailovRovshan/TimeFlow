// Services/LessonService.cs
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Enums;
using Domain.Repository;
using Microsoft.Net.Http.Headers;
using Services.Abstractions;
using Services.Abstractions.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IUserService _userService;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public LessonService(
            ILessonRepository lessonRepository,
            IUserRepository userRepository,
            IUserService userService,
            ITimeSlotRepository timeSlotRepository,
            IClientRepository clientRepository,
            ISubjectRepository subjectRepository,
            IMapper mapper,
            IEmailService emailService)
        {
            _lessonRepository = lessonRepository;
            _userRepository = userRepository;
            _userService = userService;
            _subjectRepository = subjectRepository;
            _timeSlotRepository = timeSlotRepository;
            _clientRepository = clientRepository;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<List<LessonDto>> GetAllByClientId(Guid ClientId)
        {
            var lessons = await _lessonRepository.GetAllByClientId(ClientId);
            return _mapper.Map<List<LessonDto>>(lessons);
        }

        public Task DeleteAllAsync()
            => _lessonRepository.DeleteAllAsync();

        public async Task<LessonDto> CreateAsync(LessonDtoForCreate lessonDto)
        {
            var user = await _userRepository.GetByIdAsync(lessonDto.UserId)
                ?? throw new KeyNotFoundException("Преподаватель не найден");

            var client = await _clientRepository.GetByIdAsync(lessonDto.ClientId)
                ?? throw new KeyNotFoundException("Клиент не найден");

            if (lessonDto.LessonDate < DateTime.UtcNow)
                throw new InvalidOperationException("Некорректная дата и время. Укажите дату и время, которые позже текущего момента");

            var lesson = _mapper.Map<Lesson>(lessonDto);
            await _lessonRepository.AddAsync(lesson);

            return _mapper.Map<LessonDto>(lesson);
        }

        public async Task DeleteAsync(Guid userId, Guid clientId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(userId, clientId)
                         ?? throw new KeyNotFoundException("Урок не найден.");
            await _lessonRepository.DeleteAsync(lesson);
        }

        public async Task<List<LessonDto>> GetAllAsync()
        {
            var lessons = await _lessonRepository.GetAllAsync();
            return _mapper.Map<List<LessonDto>>(lessons);
        }

        public async Task<LessonDto> GetByIdAsync(Guid userId, Guid clientId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(userId, clientId)
                         ?? throw new KeyNotFoundException("Урок не найден.");
            return _mapper.Map<LessonDto>(lesson);
        }

        public async Task UpdateAsync(Guid userId, Guid clientId, LessonDtoForUpdate lessonDto)
        {
            var lesson = await _lessonRepository.GetByIdAsync(userId, clientId)
                         ?? throw new KeyNotFoundException("Урок не найден.");

            _mapper.Map(lessonDto, lesson);
            await _lessonRepository.UpdateAsync(lesson);
        }

        public async Task<List<LessonDto>> GetLessonsByDateAsync(Guid userId, DateTime date)
        {
            var lessons = await _lessonRepository.GetLessonsByDateAsync(userId, date);
            return _mapper.Map<List<LessonDto>>(lessons);
        }

        public async Task<List<LessonDto>> GetLessonsInRangeAsync(LessonDtoInRange lessonDto)
        {
            var lessons = await _lessonRepository
                .GetLessonsInRangeAsync(lessonDto.UserId, lessonDto.startDate, lessonDto.endDate);

            return _mapper.Map<List<LessonDto>>(lessons);
        }

        public async Task<UserDto> AddRegularLessonsAsync(CreateRegularLessonsDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId)
                       ?? throw new KeyNotFoundException("Преподаватель не найден");

            var firstDates = dto.Slots.ToDictionary(
                slot => slot,
                slot =>
                {
                    var daysToAdd = ((int)slot.DayOfWeek - (int)dto.StartDate.DayOfWeek + 7) % 7;
                    var datePart = dto.StartDate.Date.AddDays(daysToAdd);
                    return DateTime.SpecifyKind(datePart.Add(slot.Time), DateTimeKind.Utc);
                });

            for (int i = 0; i < dto.Number; i++)
            {
                var slot = dto.Slots[i % dto.Slots.Count];
                var weekIndex = i / dto.Slots.Count;
                var lessonDate = firstDates[slot].AddDays(weekIndex * 7);

                var lesson = new Lesson
                {
                    Id = Guid.NewGuid(),
                    ClientId = dto.ClientId,
                    UserId = dto.UserId,
                    LessonDate = lessonDate,
                    Status = Status.Запланирован,
                    SubjectId = dto.SubjectId,
                };
                await _lessonRepository.AddAsync(lesson);
            }

            foreach (var slot in dto.Slots)
            {
                var timeSlot = await _timeSlotRepository
                    .GetByUserDayTimeAsync(dto.UserId, slot.DayOfWeek, slot.Time);

                if (timeSlot != null && timeSlot.IsBusy)
                {
                    timeSlot.IsBusy = false;
                    await _timeSlotRepository.UpdateAsync(timeSlot);
                }
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                var subjectText = "Создано расписание занятий";
                var htmlBody =
                    $@"
                    <p>Здравствуйте, {user.FullName}!</p>
                    <p>Для ученика <strong>{dto.Client.FullName} ({dto.Client.Age} лет)
                    </strong> было создано следующее расписание по предмету <strong>{dto.Subject.Name}</strong>:</p>
            <ul>
                {string.Join("", dto.Slots.Select(d => $"<li>{d:dddd, dd MMMM yyyy HH:mm}</li>"))}
            </ul>
            <p>Пожалуйста, проверьте вашу панель расписания.</p>
        ";
                await _emailService.SendEmailAsync(user.Email, subjectText, htmlBody);
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<LessonDto>> GetAllByClientIdAndDateAsync(Guid clientId, DateTime date)
        {
            var lessons = await _lessonRepository.GetAllByClientIdAndDateAsync(clientId, date);
            return _mapper.Map<List<LessonDto>>(lessons);
        }

        public async Task<UserDto> MainCreateLesson(MainCreateLessonDto lessonDtos, Guid UserId)
        {
            var user = await _userRepository.GetByIdAsync(UserId)
                ?? throw new KeyNotFoundException("Преподаватель не найден");

            var subject = await _subjectRepository.GetByIdAsync(lessonDtos.SubjectId)
                ?? throw new KeyNotFoundException("Предмет не найден"); ;

            var newClient = new Client
            {
                Id = Guid.NewGuid(),
                FullName = lessonDtos.FullName,
                Age = lessonDtos.Age
            };

            var client = await _clientRepository.AddAsync(newClient);

            var dtoForCreate = new CreateRegularLessonsDto
            (
                UserId,
                client.Id,
                _mapper.Map<ClientDto>(client),
                lessonDtos.Slots,
                lessonDtos.StartDate,
                lessonDtos.Number,
                lessonDtos.SubjectId,
                _mapper.Map<SubjectDto>(subject)

            );
            return await AddRegularLessonsAsync(dtoForCreate);
        }

        public async Task<UserDto> MainCreateLesson(MainCreateLessonDto lessonDtos, SearchMode mode)
        {
            var teachers = await _userService.GetFreeAsync(lessonDtos.Slots, lessonDtos.SubjectId);
            UserDto bestTeacher = null;

            switch (mode)
            {
                case SearchMode.TheMostFree:
                    bestTeacher = teachers
                    .OrderByDescending(t => t.TimeSlots.Count(ts => ts.IsBusy)).First() ?? throw new KeyNotFoundException("Преподаватели не найдены");
                    break;
                case SearchMode.ByExperience:
                    bestTeacher = teachers
                    .OrderByDescending(t => t.Experiense).First() ?? throw new KeyNotFoundException("Преподаватели не найдены");
                    break;
            }

            if (bestTeacher == null)
            {
                throw new KeyNotFoundException("Преподаватель null");
            }

            return await MainCreateLesson(lessonDtos, bestTeacher.Id);
        }

        public async Task DeleteLessonByIdAsync(Guid lessonId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(lessonId)
                         ?? throw new KeyNotFoundException($"Урок с Id = {lessonId} не найден.");

            lesson.Status = Status.Отменён;

            await _lessonRepository.UpdateAsync(lesson);
        }

        public async Task<LessonDto> RescheduleLessonAsync(RescheduleLessonDto dto)
        {
            var oldLesson = await _lessonRepository.GetByIdAsync(dto.LessonId)
                            ?? throw new KeyNotFoundException($"Урок с Id = {dto.LessonId} не найден.");

            oldLesson.Status = Status.Перенесён;
            await _lessonRepository.UpdateAsync(oldLesson);

            var newLesson = new Lesson
            {
                Id = Guid.NewGuid(),
                ClientId = oldLesson.ClientId,
                UserId = oldLesson.UserId,
                SubjectId = oldLesson.SubjectId,
                LessonDate = dto.NewLessonDate.ToUniversalTime(),
                Status = Status.Запланирован
            };

            await _lessonRepository.AddAsync(newLesson);

            return _mapper.Map<LessonDto>(newLesson);
        }
    }
}
