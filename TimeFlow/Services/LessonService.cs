using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Repository;
using Services.Abstractions;
using Services.Abstractions.DTO;


namespace Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public LessonService(
            ILessonRepository lessonRepository,
            IUserRepository userRepository,
            IUserService userService,
            IMapper mapper)
        {
            _lessonRepository = lessonRepository;
            _userRepository = userRepository;
            _userService = userService;
            _mapper = mapper;
        }
        public async Task DeleteAllAsync()
        {
            await _lessonRepository.DeleteAllAsync();
        }
        public async Task<LessonDto> CreateAsync(LessonDtoForCreate lessonDto)
        {
            var user = await _userRepository.GetByIdAsync(lessonDto.UserId);

            var lesson = _mapper.Map<Lesson>(lessonDto);
            await _lessonRepository.AddAsync(lesson);
            return _mapper.Map<LessonDto>(lesson);
        }

        public async Task DeleteAsync(Guid UserId, Guid ClientId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(UserId, ClientId);
            if (lesson == null)
            {
                throw new KeyNotFoundException("Урок не найден.");
            }
            await _lessonRepository.DeleteAsync(lesson);
        }

        public async Task<List<LessonDto>> GetAllAsync()
        {
            var lessons = await _lessonRepository.GetAllAsync();
            return _mapper.Map<List<LessonDto>>(lessons);
        }

        public async Task<LessonDto> GetByIdAsync(Guid UserId, Guid ClientId)
        {
            var lesson = await _lessonRepository.GetByIdAsync(UserId, ClientId);
            if (lesson == null)
            {
                throw new KeyNotFoundException("Урок не найден.");
            }
            return _mapper.Map<LessonDto>(lesson);
        }

        public async Task UpdateAsync(Guid UserId, Guid ClientId, LessonDtoForUpdate lessonDto)
        {
            var lesson = await _lessonRepository.GetByIdAsync(UserId, ClientId);

            if (lesson == null)
            {
                throw new KeyNotFoundException("Урок не найден.");
            }

            _mapper.Map(lessonDto, lesson);

            await _lessonRepository.UpdateAsync(lesson);
        }

        public async Task<List<LessonDto>> GetLessonsByDateAsync(Guid userId, DateTime date)
        {
            var lessons = await _lessonRepository.GetLessonsByDateAsync(userId, date);
            return _mapper.Map<List<LessonDto>>(lessons);
        }

        // Получение занятий за промежуток времени
        public async Task<List<LessonDto>> GetLessonsInRangeAsync(LessonDtoInRange lessonDto)
        {
            var lessons = await _lessonRepository.GetLessonsInRangeAsync(lessonDto.UserId, lessonDto.startDate, lessonDto.endDate);
            return _mapper.Map<List<LessonDto>>(lessons);
        }
        // Добавления нескольких занятий в расписанием (составление регулярного занятия)
        public async Task AddRegularLessonsAsync(LessonDtoForRegularLessons lessonDto)
        {
            var user = await _userRepository.GetByIdAsync(lessonDto.UserId);
            if (user == null)
                throw new KeyNotFoundException("Преподаватель не найден");

            var currentDate = DateTime.Today;
            int daysToAdd = ((int)lessonDto.DayOfWeek - (int)currentDate.DayOfWeek + 7) % 7;
            if (daysToAdd == 0)
                daysToAdd = 7;

            var firstLessonDate = DateTime.SpecifyKind(
                currentDate.AddDays(daysToAdd).Date + lessonDto.Time,
                DateTimeKind.Utc);

            for (int i = 0; i < lessonDto.Number; i++)
            {
                var lessonDate = firstLessonDate.AddDays(i * 7);

                var lesson = new Lesson
                {
                    Id = Guid.NewGuid(),
                    ClientId = lessonDto.ClientId,
                    UserId = lessonDto.UserId,
                    LessonDate = lessonDate,
                    Status = Status.Запланирован
                };

                await _lessonRepository.AddAsync(lesson);
            }
        }
        public async Task AutoSearch(LessonDtoForAutoAdd lessonDtoForAutoAdd)
        {
            var timeSlotDto = new TimeSlotFilterDto(
                lessonDtoForAutoAdd.DayOfWeek,
                lessonDtoForAutoAdd.Time);

            var user = await _userService.GetFreeTeacher(timeSlotDto);

            if (user == null)
            {
                throw new InvalidOperationException("Нет свободных преподавателей для выбранного времени.");
            }

            var regularLessonDto = new LessonDtoForRegularLessons(
                    user.Id,
                    lessonDtoForAutoAdd.ClientId,
                    lessonDtoForAutoAdd.DayOfWeek,
                    lessonDtoForAutoAdd.Time,
                    lessonDtoForAutoAdd.Number
                );

            await AddRegularLessonsAsync(regularLessonDto);
        }

    }
}
