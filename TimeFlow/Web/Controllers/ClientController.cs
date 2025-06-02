using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.DTO;
using Services.Abstractions;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(clients);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetClientsAsync(string name)
        {
            var clients = await _clientService.SearchClientsAsync(name);
            return Ok(clients);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var client = await _clientService.GetByIdAsync(id);
            return Ok(client);
        }

        [Authorize(Roles = "Manager")]
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

        [Authorize(Roles = "Manager")]
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

        [Authorize(Roles = "Manager")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _clientService.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAllAsync()
        {
            await _clientService.DeleteAllAsync();
            return NoContent();
        }
    }
}
