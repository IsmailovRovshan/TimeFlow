
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public TimeSlotRepository(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TimeSlot timeSlot)
        {
            await _dbContext.TimeSlots.AddAsync(timeSlot);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TimeSlot timeSlot)
        {
            _dbContext.TimeSlots.Remove(timeSlot);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<TimeSlot>> GetAllAsync()
        {
            return await _dbContext.TimeSlots
                .Include(t => t.Teachers)
                .ToListAsync();
        }

        public async Task<TimeSlot> GetByIdAsync(Guid id)
        {
            return await _dbContext.TimeSlots
                .Include(t => t.Teachers)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateAsync(TimeSlot timeSlot)
        {
            _dbContext.TimeSlots.Update(timeSlot);
            await _dbContext.SaveChangesAsync();
        }
    }
}
