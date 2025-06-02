using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public SubjectRepository(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Subject subject)
        {
            await _dbContext.Subjects.AddAsync(subject);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Subject subject)
        {
            _dbContext.Subjects.Remove(subject);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Subject>> GetAllAsync()
        {
            return await _dbContext.Subjects
                .Include(m => m.Users)
                .Include(l => l.Lessons)
                .ToListAsync();
        }

        public async Task<Subject> GetByIdAsync(Guid id)
        {
            return await _dbContext.Subjects
                .Include(m => m.Users)
                .Include(l => l.Lessons)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task UpdateAsync(Subject subject)
        {
            _dbContext.Subjects.Update(subject);
            await _dbContext.SaveChangesAsync();
        }


        public async Task DeleteAllAsync()
        {
            _dbContext.Subjects.RemoveRange(_dbContext.Subjects);
            await _dbContext.SaveChangesAsync();
        }
    }
}
