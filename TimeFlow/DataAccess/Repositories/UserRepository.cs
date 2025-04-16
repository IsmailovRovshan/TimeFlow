using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RepositoryDbContext _dbContext;
        public UserRepository(RepositoryDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dbContext.Users
                .Include(t => t.TimeSlots)
                .Include(l => l.Lessons)
                .ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _dbContext.Users
                .Include(t => t.TimeSlots)
                .Include(l => l.Lessons)
                .Include(l => l.Subjects)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        
        public async Task<List<User>> GetFreeAsync(DayOfWeek dayOfWeek, TimeSpan time)
        {
            return await _dbContext.Users
                .Include(u => u.TimeSlots)
                .Where(u => u.Role == Role.Teacher)
                .Where(u => u.TimeSlots.Any(ts =>
                    ts.DayOfWeek == dayOfWeek &&
                    ts.Time == time &&
                    ts.IsBusy))
                .OrderBy(u => u.TimeSlots
                .Count(ts => ts.DayOfWeek == dayOfWeek && ts.Time == time && ts.IsBusy))
                .ToListAsync();
        }

        public async Task DeleteAllAsync()
        {
            _dbContext.Users.RemoveRange(_dbContext.Users);
            await _dbContext.SaveChangesAsync();
        }

    }
}
