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

        public LessonService(
            ILessonRepository lessonRepository,
            IUserRepository userRepository,
            IUserService userService,
            ITimeSlotRepository timeSlotRepository,
            IClientRepository clientRepository,
            ISubjectRepository subjectRepository,
            IMapper mapper)
        {
            _lessonRepository = lessonRepository;
            _userRepository = userRepository;
            _userService = userService;
            _subjectRepository = subjectRepository;
            _timeSlotRepository = timeSlotRepository;
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public Task DeleteAllAsync()
            => _lessonRepository.DeleteAllAsync();

        public async Task<LessonDto> CreateAsync(LessonDtoForCreate lessonDto)
        {
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
        // Создание уроков в количестве указанном 
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
                    Status = Status.Запланирован
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
            return _mapper.Map<UserDto>(user);
        }

        //Основной метод создания (преподаватель - вручную)
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
                lessonDtos.Slots,
                lessonDtos.StartDate,
                lessonDtos.Number
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

            if (bestTeacher == null) {
                throw new KeyNotFoundException("Преподаватель null");
            }

            return await MainCreateLesson(lessonDtos, bestTeacher.Id);
        }
    }
}
