using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.DTO;
using Services.Abstractions;
using Services;

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

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var subjects = await _subjectService.GetAllAsync();
            return Ok(subjects);
        }

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

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] SubjectDtoForUpdate subjectDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _subjectService.UpdateAsync(id, subjectDto);
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
                await _subjectService.DeleteAsync(id);
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
            await _subjectService.DeleteAllAsync();
            return NoContent();
        }
    }
}
