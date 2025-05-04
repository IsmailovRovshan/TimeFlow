using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Abstractions;
using Services.Abstractions.DTO;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound("Преподаватель не найден.");
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UserDtoForCreate userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUser = await _userService.CreateAsync(userDto);
            return Ok(newUser);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UserDtoForUpdate userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.UpdateAsync(id, userDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Клиент не найден.");
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("не найден.");
            }

            return NoContent();
        }

        [HttpPost("free")]
        public async Task<IActionResult> GetFreeTeachersAsync(
             [FromBody] List<TimeSlotDtoDateWithTime> requestedSlots)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var freeTeachers = await _userService.GetFreeAsync(requestedSlots);
            return Ok(freeTeachers);
        }

        [HttpPost("most-free")]
        public async Task<IActionResult> GetFreeTeacherAsync([FromBody] TimeSlotDtoDateWithTime timeSlotDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
          //  var freeTeachers = await _userService.GetFreeTeacher(timeSlotDto);
            return Ok(null);

            // TODO 

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllAsync()
        {
            await _userService.DeleteAllAsync();
            return NoContent();
        }
    }
}
