using Domain.Entities;

namespace Domain.Repository
{
    public interface ITimeSlotRepository
    {
        Task<TimeSlot> GetByIdAsync(Guid id);
        Task<List<TimeSlot>> GetAllAsync();
        Task AddAsync(TimeSlot timeSlot);
        Task UpdateAsync(TimeSlot timeSlot);
        Task DeleteAsync(TimeSlot timeSlot);
    }
}
