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
    public class ManagerRepository : IManagerRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public ManagerRepository(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Manager manager)
        {
            await _dbContext.Managers.AddAsync(manager);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Manager manager)
        {
            _dbContext.Managers.Remove(manager);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Manager>> GetAllAsync()
        {
            return await _dbContext.Managers
                .Include(m => m.Clients)  
                .ToListAsync();
        }

        public async Task<Manager> GetByIdAsync(Guid id)
        {
            return await _dbContext.Managers
                .Include(m => m.Clients)   
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task UpdateAsync(Manager manager)
        {
            _dbContext.Managers.Update(manager);
            await _dbContext.SaveChangesAsync();
        }
    }
}
