using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using Services.Abstractions;
using Services.Abstractions.DTO;


namespace Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _managerRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ILessonRepository _lessonRepository;

        private readonly IMapper _mapper;

        public ManagerService
            (IManagerRepository managerRepository,
            ITeacherRepository teacherRepository,
            ILessonRepository lessonRepository,
            IMapper mapper)
        {
            _managerRepository = managerRepository;
            _teacherRepository = teacherRepository;
            _lessonRepository = lessonRepository;
            _mapper = mapper;
        }

        public async Task<ManagerDto> CreateAsync(ManagerDtoForCreate managerDto)
        {
            var manager = _mapper.Map<Manager>(managerDto);
            await _managerRepository.AddAsync(manager);
            return _mapper.Map<ManagerDto>(manager);
        }

        public async Task DeleteAsync(Guid managerId)
        {
            var manager = await _managerRepository.GetByIdAsync(managerId);

            if (manager == null)
            {
                throw new ArgumentException("Менеджер не найден.");
            }

            await _managerRepository.DeleteAsync(manager);
        }

        public async Task<List<ManagerDto>> GetAllAsync()
        {
            var managers = await _managerRepository.GetAllAsync();

            return _mapper.Map<List<ManagerDto>>(managers);
        }

        public async Task<ManagerDto> GetByIdAsync(Guid id)
        {
            var manager = await _managerRepository.GetByIdAsync(id);

            if (manager == null)
            {
                throw new ArgumentException("Менеджер не найден.");
            }

            return _mapper.Map<ManagerDto>(manager);
        }

        public async Task UpdateAsync(Guid managerId, ManagerDtoForUpdate managerDto)
        {
            var existingManager = await _managerRepository.GetByIdAsync(managerId);

            if (existingManager == null)
            {
                throw new ArgumentException("Менеджер не найден.");
            }

            _mapper.Map(managerDto, existingManager);

            await _managerRepository.UpdateAsync(existingManager);
        }
    }
}
