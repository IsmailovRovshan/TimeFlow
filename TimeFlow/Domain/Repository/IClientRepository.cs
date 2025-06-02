using Domain.Entities;

namespace Domain.Repository
{
    public interface IClientRepository
    {
        Task<Client> GetByIdAsync(Guid id);
        Task<List<Client>> GetAllAsync();
        Task<Client> AddAsync(Client client);
        Task UpdateAsync(Client client);
        Task DeleteAsync(Client client);

        Task DeleteAllAsync();
        Task<List<Client>> SearchClientsAsync(string name);
    }
}
