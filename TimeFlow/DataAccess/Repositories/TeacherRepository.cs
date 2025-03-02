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
    public class TeacherRepository : ITeacherRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public TeacherRepository(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Teacher teacher)
        {
            await _dbContext.Teachers.AddAsync(teacher);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Teacher teacher)
        {
            _dbContext.Teachers.Remove(teacher);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Teacher>> GetAllAsync()
        {
            return await _dbContext.Teachers.ToListAsync();
        }

        public async Task<Teacher> GetByIdAsync(Guid id)
        {
            return await _dbContext.Teachers
                .Include(t => t.Lessons)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateAsync(Teacher teacher)
        {
            _dbContext.Teachers.Update(teacher);
            await _dbContext.SaveChangesAsync();
        }

    }
}
