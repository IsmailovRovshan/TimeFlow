using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public ClientRepository(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Client client)
        {
            await _dbContext.Clients.AddAsync(client);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Client client)
        {
            _dbContext.Clients.Remove(client);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Client>> GetAllAsync()
        {
            return await _dbContext.Clients
                .Include(c => c.Lessons)
                .ToListAsync();
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            return await _dbContext.Clients
                .Include(c => c.Lessons)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(Client client)
        {
            _dbContext.Clients.Update(client);
            await _dbContext.SaveChangesAsync();
        }
    }
}
