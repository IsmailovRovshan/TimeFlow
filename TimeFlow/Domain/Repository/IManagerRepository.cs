using Domain.Entities;

namespace Domain.Repository
{
    public interface IManagerRepository
    {
        Task<Manager> GetByIdAsync(Guid id);
        Task<List<Manager>> GetAllAsync();
        Task AddAsync(Manager manager);
        Task UpdateAsync(Manager manager);
        Task DeleteAsync(Manager manager);
    }
}
