// Services/LessonService.cs
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Repository;
using Services.Abstractions;
using Services.Abstractions.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public LessonService(
            ILessonRepository lessonRepository,
            IUserRepository userRepository,
            IUserService userService,
            ITimeSlotRepository timeSlotRepository,
            IMapper mapper)
        {
            _lessonRepository = lessonRepository;
            _userRepository = userRepository;
            _userService = userService;
            _timeSlotRepository = timeSlotRepository;
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

        public async Task AddRegularLessonsAsync(CreateRegularLessonsDto dto)
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
                if (timeSlot != null && !timeSlot.IsBusy)
                {
                    timeSlot.IsBusy = true;
                    await _timeSlotRepository.UpdateAsync(timeSlot);
                }
            }
        }


        public async Task<UserDto> AutoSearch(LessonDtoForAutoAdd lessonDtoForAutoAdd)
        {
            //var timeSlotDto = new TimeSlotFilterDto(
            //    lessonDtoForAutoAdd.DayOfWeek,
            //    lessonDtoForAutoAdd.Time);

            //// теперь GetFreeTeacher возвращает UserDto
            //var userDto = await _userService.GetFreeTeacher(timeSlotDto);
            //if (userDto == null)
            //    throw new InvalidOperationException("Нет свободных преподавателей для выбранного времени.");

            //var regularDto = new LessonDtoForRegularLessons(
            //    userDto.Id,
            //    lessonDtoForAutoAdd.ClientId,
            //    lessonDtoForAutoAdd.DayOfWeek,
            //    lessonDtoForAutoAdd.Time,
            //    lessonDtoForAutoAdd.Number);

            //await AddRegularLessonsAsync(regularDto);
            //return userDto;

            return null;

            // TODO 
        }

        public async Task<UserDto> AutoSearchMulti(IEnumerable<LessonDtoForAutoAdd> lessonDtos)
        {
            //    var requestedSlots = lessonDtos
            //        .Select(dto => new TimeSlot
            //        {
            //            DayOfWeek = dto.DayOfWeek,
            //            Time = dto.Time
            //        })
            //        .ToList();

            //    // репозиторий ищет User по всем слотам разом
            //    var teacher = (await _userRepository.GetFreeAsync(requestedSlots))
            //                  .FirstOrDefault()
            //                  ?? throw new InvalidOperationException("Нет преподавателя с такими слотами.");

            //    foreach (var dto in lessonDtos)
            //    {
            //        var regularDto = new LessonDtoForRegularLessons(
            //            teacher.Id,
            //            dto.ClientId,
            //            dto.DayOfWeek,
            //            dto.Time,
            //            dto.Number);
            //        await AddRegularLessonsAsync(regularDto);
            //    }

            //    return _mapper.Map<UserDto>(teacher);
            //}
            return null;
        }
    }
}
