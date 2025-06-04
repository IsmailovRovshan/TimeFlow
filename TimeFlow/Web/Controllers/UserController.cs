using Microsoft.AspNetCore.Authorization;
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
            var user = await _userService.UpdateAsync(id, userDto);
            return Ok(user);
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
        [Authorize(Roles = "Manager,Teacher")]
        [HttpPost("free")]
        public async Task<IActionResult> GetFreeTeachersAsync(
        [FromQuery] Guid subjectId,
        [FromBody] List<TimeSlotDtoDateWithTime> requestedSlots)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var freeTeachers = await _userService.GetFreeAsync(requestedSlots, subjectId);
            return Ok(freeTeachers);
        }


        [Authorize(Roles = "Manager,Teacher")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAllAsync()
        {
            await _userService.DeleteAllAsync();
            return NoContent();
        }
        [Authorize(Roles = "Manager,Teacher")]
        [HttpPost("{userId:guid}/subjects/{subjectId:guid}")]
        public async Task<IActionResult> AddSubjectToUser(Guid userId, Guid subjectId)
        {
            await _userService.AddSubjectToUserAsync(userId, subjectId);
            return NoContent();
        }

        [Authorize(Roles = "Manager,Teacher")]
        [HttpDelete("{userId:guid}/subjects/{subjectId:guid}")]
        public async Task<IActionResult> RemoveSubjectFromUser(Guid userId, Guid subjectId)
        {
            await _userService.RemoveSubjectFromUserAsync(userId, subjectId);
            return NoContent();
        }
    }
}
