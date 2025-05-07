using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.DTO;
using Services.Abstractions;
using Services;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/timeSlots")]
    public class TimeSlotController : ControllerBase
    {
        private readonly ITimeSlotService _timeSlotService;

        public TimeSlotController(ITimeSlotService timeSlotService)
        {
            _timeSlotService = timeSlotService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var timeSlots = await _timeSlotService.GetAllAsync();
            return Ok(timeSlots);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var timeSlot = await _timeSlotService.GetByIdAsync(id);
            if (timeSlot == null)
            {
                return NotFound("Предмет не найден.");
            }

            return Ok(timeSlot);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] TimeSlotDtoForCreate timeSlotDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newTimeSlot = await _timeSlotService.CreateAsync(timeSlotDto);
            return Ok(newTimeSlot);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] TimeSlotDtoForUpdate timeSlotDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _timeSlotService.UpdateAsync(id, timeSlotDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Предмет не найден.");
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await _timeSlotService.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Предмет не найден.");
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllAsync()
        {
            await _timeSlotService.DeleteAllAsync();
            return NoContent();
        }

        [HttpGet("free/{id:guid}")]
        public async Task<IActionResult> GetFreeByUserAsync(Guid id)
        {
            var timeSlots = await _timeSlotService.GetFreeTimeSlotsByUser(id);
            return Ok(timeSlots);
        }
    }
}
