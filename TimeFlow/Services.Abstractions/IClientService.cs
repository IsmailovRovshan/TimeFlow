using Services.Abstractions.DTO;

namespace Services.Abstractions
{
    public interface IClientService
    {
        Task<ClientDto> GetByIdAsync(Guid id);
        Task<List<ClientDto>> GetAllAsync();
        Task<ClientDto> CreateAsync(ClientDtoForCreate clientDto);
        Task UpdateAsync(Guid clientId, ClientDtoForUpdate client);
        Task DeleteAsync(Guid clientId);
    }
}
