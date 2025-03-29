
using Services.Abstractions.DTO;

namespace Services.Abstractions
{
    public interface ITimeSlotService
    {
        Task<TimeSlotDto> GetByIdAsync(Guid id);
        Task<List<TimeSlotDto>> GetAllAsync();
        Task<TimeSlotDto> CreateAsync(TimeSlotDtoForCreate managerDto);
        Task UpdateAsync(Guid managerId, TimeSlotDtoForUpdate manager);
        Task DeleteAsync(Guid managerId);
    }
}
