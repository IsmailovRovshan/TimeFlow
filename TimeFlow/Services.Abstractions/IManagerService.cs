using Services.Abstractions.DTO;


namespace Services.Abstractions
{
    public interface IManagerService
    {
        Task<ManagerDto> GetByIdAsync(Guid id);
        Task<List<ManagerDto>> GetAllAsync();
        Task<ManagerDto> CreateAsync(ManagerDtoForCreate managerDto);
        Task UpdateAsync(Guid managerId, ManagerDtoForUpdate manager);
        Task DeleteAsync(Guid managerId);
    }
}
