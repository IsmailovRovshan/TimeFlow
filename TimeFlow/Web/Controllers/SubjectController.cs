using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.DTO;
using Services.Abstractions;
using Services;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/subjects")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [Authorize(Roles = "Manager,Teacher")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var subjects = await _subjectService.GetAllAsync();
            return Ok(subjects);
        }
        [Authorize(Roles = "Manager,Teacher")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var subject = await _subjectService.GetByIdAsync(id);
            if (subject == null)
            {
                return NotFound("Предмет не найден.");
            }

            return Ok(subject);
        }
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] SubjectDtoForCreate subjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newSubject = await _subjectService.CreateAsync(subjectDto);
            return Ok(newSubject);
        }
        [Authorize(Roles = "Manager")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] SubjectDtoForUpdate subjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _subjectService.UpdateAsync(id, subjectDto);
            return NoContent();
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _subjectService.DeleteAsync(id);
            return NoContent();
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAllAsync()
        {
            await _subjectService.DeleteAllAsync();
            return NoContent();
        }
    }
}
