
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
                .Include(t => t.User)
                .OrderBy(t => t.DayOfWeek)
                .ToListAsync();
        }

        public async Task<TimeSlot> GetByIdAsync(Guid id)
        {
            return await _dbContext.TimeSlots
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateAsync(TimeSlot timeSlot)
        {
            _dbContext.TimeSlots.Update(timeSlot);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAllAsync()
        {
            _dbContext.TimeSlots.RemoveRange(_dbContext.TimeSlots);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TimeSlot?> GetByUserDayTimeAsync(Guid userId, DayOfWeek dayOfWeek, TimeSpan time)
        {
            return await _dbContext.TimeSlots
                .FirstOrDefaultAsync(ts =>
                    ts.UserId == userId &&
                    ts.DayOfWeek == dayOfWeek &&
                    ts.Time == time);
        }

        public async Task<List<TimeSlot>> GetFreeTimeSlotsByUser(Guid userId)
        {
            return await _dbContext.TimeSlots
                .Where(t => t.UserId == userId && t.IsBusy)
                .ToListAsync();
        }
    }
}
