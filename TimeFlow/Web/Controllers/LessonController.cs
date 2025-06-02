using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.DTO;
using Services.Abstractions;
using Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/lessons")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lessons = await _lessonService.GetAllAsync();
            return Ok(lessons);
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("{teacherId:guid}/{clientId:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid teacherId, Guid clientId)
        {
            var lesson = await _lessonService.GetByIdAsync(teacherId, clientId);
            if (lesson == null)
            {
                return NotFound("Урок не найден.");
            }

            return Ok(lesson);
        }
        
        [Authorize(Roles = "Manager,Teacher")]
        [HttpGet("client/{clientId:guid}")]
        public async Task<IActionResult> GetLessonsByClient(
            [FromRoute] Guid clientId,
            [FromQuery] DateTime? date)             
        {
            List<LessonDto> lessons;

            if (date.HasValue)
            {
                lessons = await _lessonService.GetAllByClientIdAndDateAsync(clientId, date.Value);
            }
            else
            {
                lessons = await _lessonService.GetAllByClientId(clientId);
            }

            if (lessons == null || lessons.Count == 0)
                return NotFound("Уроки не найдены.");

            return Ok(lessons);
        }
        
        [Authorize(Roles = "Manager,Teacher")]
        [HttpDelete("{lessonId:guid}")]
        public async Task<IActionResult> DeleteLessonByIdAsync(
            [FromRoute] Guid lessonId)
        {
            await _lessonService.DeleteLessonByIdAsync(lessonId);
            return NoContent();
        }
        
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] LessonDtoForCreate lessonDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdLesson = await _lessonService.CreateAsync(lessonDto);
            return Ok(createdLesson);
        }
        [Authorize(Roles = "Manager")]

        [HttpPut("{teacherId:guid}/{clientId:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid teacherId, Guid clientId, [FromBody] LessonDtoForUpdate lessonDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _lessonService.UpdateAsync(teacherId, clientId, lessonDto);
            return NoContent();

        }
        [Authorize(Roles = "Manager")]
        [HttpDelete("{teacherId:guid}/{clientId:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid teacherId, Guid clientId)
        {
            await _lessonService.DeleteAsync(teacherId, clientId);
            return NoContent();
        }
        [Authorize(Roles = "Manager,Teacher")]
        [HttpGet("{teacherId:guid}/lessons")]
        public async Task<IActionResult> GetLessonsByDateAsync(Guid teacherId, [FromQuery] DateTime date)
        {
            var lessons = await _lessonService.GetLessonsByDateAsync(teacherId, date);
            if (lessons == null || lessons.Count == 0)
            {
                return NotFound("Уроки на указанную дату не найдены.");
            }

            return Ok(lessons);
        }
        [Authorize(Roles = "Manager,Teacher")]
        [HttpPost("range")]
        public async Task<IActionResult> GetLessonsInRangeAsync([FromBody] LessonDtoInRange lessonDto)
        {
            var lessons = await _lessonService.GetLessonsInRangeAsync(lessonDto);
            return Ok(lessons);
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAllAsync()
        {
            await _lessonService.DeleteAllAsync();
            return NoContent();
        }
        [Authorize(Roles = "Manager")]
        [HttpPost("regular")]
        public async Task<IActionResult> AddRegularLessonsAsync(
            [FromBody] CreateRegularLessonsDto lessonDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var lesson = await _lessonService.AddRegularLessonsAsync(lessonDto);
            return Ok(lesson);
        }
        [Authorize(Roles = "Manager")]
        [HttpPost("main-create")]
        public async Task<IActionResult> MainCreateLesson([FromBody] MainCreateLessonDto dto, Guid userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var lesson = await _lessonService.MainCreateLesson(dto, userId);
            return Ok(lesson);
        }
        [Authorize(Roles = "Manager")]
        [HttpPost("main-create/{mode}")]
        public async Task<IActionResult> MainCreateLessonByMode([FromRoute] SearchMode mode, [FromBody] MainCreateLessonDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var lesson = await _lessonService.MainCreateLesson(dto, mode);
            return Ok(lesson);
        }
        [Authorize(Roles = "Manager,Teacher")]
        [HttpPost("reschedule")]
        public async Task<IActionResult> RescheduleLessonAsync([FromBody] RescheduleLessonDto dto)
        {
            if (dto == null)
                return BadRequest("Не указаны данные для переноса.");

            var createdLessonDto = await _lessonService.RescheduleLessonAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync),
                new { userId = createdLessonDto.UserId, clientId = createdLessonDto.ClientId },
                createdLessonDto);
        }
        

    }
}
