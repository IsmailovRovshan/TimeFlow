using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccess.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public LessonRepository(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Lesson lesson)
        {
            await _dbContext.Lessons.AddAsync(lesson);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Lesson lesson)
        {
            _dbContext.Lessons.Remove(lesson);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Lesson>> GetAllByClientId(Guid ClientId)
        {
            return await _dbContext.Lessons
                .Include(l => l.User)
                .Include(l => l.Client)
                .Include(l => l.Subject)
                .Where(l => l.ClientId == ClientId)
                .OrderByDescending(l => l.LessonDate)
                .ToListAsync();
        }

        public async Task<Lesson> GetByIdAsync(Guid Id)
        {
            return await _dbContext.Lessons
                    .Include(l => l.User)
                    .Include(l => l.Client)
                    .Include(l => l.Subject)
                    .FirstOrDefaultAsync(l => l.Id == Id);
        }

        public async Task<List<Lesson>> GetAllAsync()
        {
            return await _dbContext.Lessons
                .Include(l => l.User)
                .Include(l => l.Client)
                .Include(l => l.Subject)
                .ToListAsync();
        }

        public async Task<Lesson> GetByIdAsync(Guid TeacherId, Guid ClientId)
        {
            return await _dbContext.Lessons
                .Include(l => l.User)
                .Include(l => l.Client)
                .Include(l => l.Subject)
                .FirstOrDefaultAsync(l => l.UserId == TeacherId && l.ClientId == ClientId);
        }

        public async Task UpdateAsync(Lesson lesson)
        {
            _dbContext.Lessons.Update(lesson);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Lesson>> GetLessonsByDateAsync(Guid teacherId, DateTime date)
        {
            var lessons = await _dbContext.Lessons
                    .Include(l => l.User)
                    .Include(l => l.Client)
                    .Include(l => l.Subject)
                    .Where(l => l.UserId == teacherId &&
                    l.LessonDate.Year == date.Year &&
                    l.LessonDate.Month == date.Month &&
                    l.LessonDate.Day == date.Day)
                    .ToListAsync();

            return lessons;
        }

        public async Task<List<Lesson>> GetLessonsInRangeAsync(Guid UserId, DateTime startDate, DateTime endDate)
        {
            return await _dbContext.Lessons
            .Include(l => l.User)
            .Include(l => l.Client)
            .Include(l => l.Subject)
            .Where(l => l.LessonDate >= startDate && l.LessonDate <= endDate && l.UserId == UserId)
            .ToListAsync();
        }

        public async Task<List<Lesson>> GetAllByClientIdAndDateAsync(Guid clientId, DateTime date)
        {
            return await _dbContext.Lessons
                .Include(l => l.User)
                .Include(l => l.Client)
                .Include(l => l.Subject)
                .Where(l =>
                    l.ClientId == clientId &&
                    l.LessonDate.Year  == date.Year &&
                    l.LessonDate.Month == date.Month &&
                    l.LessonDate.Day   == date.Day
                )
                .OrderByDescending(l => l.LessonDate)
                .ToListAsync();
        }

        public Task AddRegularLessonsAsync(DayOfWeek DayOfWeek, TimeSpan Time, int Number)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAllAsync()
        {
            _dbContext.Lessons.RemoveRange(_dbContext.Lessons);
            await _dbContext.SaveChangesAsync();
        }
    }
}
