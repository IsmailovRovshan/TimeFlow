using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.DTO;
using Services.Abstractions;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(clients);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var client = await _clientService.GetByIdAsync(id);
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ClientDtoForCreate clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newClient = await _clientService.CreateAsync(clientDto);
            return Ok(newClient);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ClientDtoForUpdate clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _clientService.UpdateAsync(id, clientDto);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _clientService.DeleteAsync(id);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllAsync()
        {
            await _clientService.DeleteAllAsync();
            return NoContent();
        }
    }
}
