using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using Services.Abstractions;
using Services.Abstractions.DTO;

namespace Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ILessonRepository _lessonRepository;

        private readonly IMapper _mapper;

        public ClientService
            (IClientRepository clientRepository,
            IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<ClientDto> CreateAsync(ClientDtoForCreate clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            await _clientRepository.AddAsync(client);
            return _mapper.Map<ClientDto>(client);
        }

        public async Task DeleteAsync(Guid clientId)
        {
            var client = await _clientRepository.GetByIdAsync(clientId);
            if (client == null)
            {
                throw new KeyNotFoundException("Client not found.");
            }

            await _clientRepository.DeleteAsync(client);
        }

        public async Task<List<ClientDto>> GetAllAsync()
        {
            var clients = await _clientRepository.GetAllAsync();

            return _mapper.Map<List<ClientDto>>(clients);
        }


        public async Task<ClientDto> GetByIdAsync(Guid id)
        {
            var client = await _clientRepository.GetByIdAsync(id);

            if (client == null)
            {
                throw new ArgumentException("Клиент не найден.");
            }

            return _mapper.Map<ClientDto>(client);
        }

        public async Task UpdateAsync(Guid clientId, ClientDtoForUpdate client)
        {
            var existingClient = await _clientRepository.GetByIdAsync(clientId);

            if (existingClient == null)
            {
                throw new KeyNotFoundException("Client not found.");
            }

            existingClient.FullName = client.FullName;
            existingClient.Age = client.Age;

            await _clientRepository.UpdateAsync(existingClient);
        }
    }
}
