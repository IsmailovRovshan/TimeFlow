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
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository teacherRepository, ISubjectRepository subjectRepository , IMapper mapper)
        {
            _teacherRepository = teacherRepository;
            _subjectRepository = subjectRepository;
            _mapper = mapper;
        }

        public Task DeleteAllAsync()
            => _teacherRepository.DeleteAllAsync();

        public async Task<UserDto> CreateAsync(UserDtoForCreate teacherDto)
        {
            if (await _teacherRepository.GetByLoginAsync(teacherDto.Login) is not null)
                throw new InvalidOperationException($"Пользователь с логином «{teacherDto.Login}» уже существует.");

            if (await _teacherRepository.GetByEmailAsync(teacherDto.Email) is not null)
                throw new InvalidOperationException($"Пользователь с email «{teacherDto.Email}» уже существует.");

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
            var teacher = await _teacherRepository.GetByIdAsync(id)
                          ?? throw new KeyNotFoundException("User not found");
            return _mapper.Map<UserDto>(teacher);
        }

        public async Task<UserDto> UpdateAsync(Guid teacherId, UserDtoForUpdate teacherDto)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId)
                          ?? throw new KeyNotFoundException("User not found");

            _mapper.Map(teacherDto, teacher);
            await _teacherRepository.UpdateAsync(teacher);

            return _mapper.Map<UserDto>(teacher);
        }

        public async Task DeleteAsync(Guid teacherId)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId)
                          ?? throw new KeyNotFoundException("User not found");
            await _teacherRepository.DeleteAsync(teacher);
        }


        //public Task<List<UserDto>> GetFreeAsync(TimeSlotFilterDto timeSlotDto)
        //    => GetFreeAsync(new[] { timeSlotDto });

        //public async Task<UserDto> GetFreeTeacher(TimeSlotFilterDto timeSlotDto)
        //{
        //    var free = await GetFreeAsync(timeSlotDto);
        //    return free.FirstOrDefault();
        //}

        public async Task<List<UserDto>> GetFreeAsync(IEnumerable<TimeSlotDtoDateWithTime> requestedSlots, Guid subjectId)
        {
            var slotEntities = _mapper.Map<List<TimeSlot>>(requestedSlots);

            var users = await _teacherRepository.GetFreeAsync(slotEntities, subjectId);
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task AddSubjectToUserAsync(Guid userId, Guid subjectId)
        {
            var user = await _teacherRepository.GetByIdAsync(userId) ?? throw new KeyNotFoundException($"Преподаватель {userId} не найден");
            var subject = await _subjectRepository.GetByIdAsync(subjectId) ?? throw new KeyNotFoundException($"Предмет {subjectId} не найден"); ;

            if (user.Subjects.Any(s => s.Id == subjectId))
                return; 

            await _teacherRepository.AddSubjectToUserAsync(user, subject);
        }

        public async Task RemoveSubjectFromUserAsync(Guid userId, Guid subjectId)
        {
            var user = await _teacherRepository.GetByIdAsync(userId) ?? throw new KeyNotFoundException($"Преподаватель {userId} не найден");
            var subject = await _subjectRepository.GetByIdAsync(subjectId) ?? throw new KeyNotFoundException($"Предмет {subjectId} не найден");

            if (!user.Subjects.Any(s => s.Id == subjectId))
                 throw new InvalidOperationException("Этот предмет не привязан к преподавателю");

            await _teacherRepository.RemoveSubjectFromUserAsync (user, subject);
        }

        //public async Task<UserDto> GetFreeTeacher(IEnumerable<TimeSlotFilterDto> requestedSlots)
        //{
        //    var free = await GetFreeAsync(requestedSlots);
        //    return free.FirstOrDefault()
        //           ?? throw new InvalidOperationException("Нет преподавателя с такими слотами.");
        //}
    }
}
