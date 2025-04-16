using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using Services.Abstractions;
using Services.Abstractions.DTO;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _teacherRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository teacherRepository, ILessonRepository lessonRepository, IMapper mapper)
        {
            _teacherRepository = teacherRepository;
            _lessonRepository = lessonRepository;
            _mapper = mapper;
        }
        public async Task DeleteAllAsync()
        {
            await _teacherRepository.DeleteAllAsync();
        }
        public async Task<UserDto> CreateAsync(UserDtoForCreate teacherDto)
        {
            var teacher = _mapper.Map<User>(teacherDto);
            await _teacherRepository.AddAsync(teacher);
            return _mapper.Map<UserDto>(teacher);
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var teachers = await _teacherRepository.GetAllAsync();
            return _mapper.Map<List<UserDto>>(teachers);
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var teacher = await _teacherRepository.GetByIdAsync(id);

            if (teacher == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return _mapper.Map<UserDto>(teacher);
        }

        public async Task UpdateAsync(Guid teacherId, UserDtoForUpdate teacherDto)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId);

            if (teacher == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            _mapper.Map(teacherDto, teacher);
            await _teacherRepository.UpdateAsync(teacher);
        }

        public async Task DeleteAsync(Guid teacherId)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId);

            if (teacher == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            await _teacherRepository.DeleteAsync(teacher);
        }
        // Список всех свободных
        public async Task<List<UserDto>> GetFreeAsync(TimeSlotFilterDto timeSlotDto)
        {
            var freeUsers = await _teacherRepository
                .GetFreeAsync(timeSlotDto.DayOfWeek,timeSlotDto.Time);

            if (freeUsers == null)
            {
                return new List<UserDto>();
            }

            return _mapper.Map<List<UserDto>>(freeUsers);

        }
        // Получение "самого" свободного 
        public async Task<UserDto> GetFreeTeacher(TimeSlotFilterDto timeSlotDto)
        {
            var freeUsers = await GetFreeAsync(timeSlotDto);
            var user = freeUsers.FirstOrDefault();
            return _mapper.Map<UserDto>(user);
        }
    }
}
