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

        public async Task<List<Lesson>> GetAllAsync()
        {
            return await _dbContext.Lessons
                .Include(l => l.Teacher)
                .Include(l => l.Client)
                .ToListAsync();
        }

        public async Task<Lesson> GetByIdAsync(Guid TeacherId, Guid ClientId)
        {
            return await _dbContext.Lessons
                .Include(l => l.Teacher)
                .Include(l => l.Client)
                .FirstOrDefaultAsync(l => l.TeacherId == TeacherId && l.ClientId == ClientId);
        }

        public async Task UpdateAsync(Lesson lesson)
        {
            _dbContext.Lessons.Update(lesson);
            await _dbContext.SaveChangesAsync();
        }
    }
}
