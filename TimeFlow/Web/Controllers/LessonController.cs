using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.DTO;
using Services.Abstractions;
using Services;

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

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lessons = await _lessonService.GetAllAsync();
            return Ok(lessons);
        }

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

        [HttpPut("{teacherId:guid}/{clientId:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid teacherId, Guid clientId, [FromBody] LessonDtoForUpdate lessonDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _lessonService.UpdateAsync(teacherId, clientId, lessonDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Урок не найден.");
            }
        }

        [HttpDelete("{teacherId:guid}/{clientId:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid teacherId, Guid clientId)
        {
            try
            {
                await _lessonService.DeleteAsync(teacherId, clientId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Урок не найден.");
            }
        }

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

        [HttpPost("range")]
        public async Task<IActionResult> GetLessonsInRangeAsync([FromBody] LessonDtoInRange lessonDto)
        {
            var lessons = await _lessonService.GetLessonsInRangeAsync(lessonDto);
            return Ok(lessons);
        }

        [HttpPost("regular")]
        public async Task<IActionResult> AddRegularLessonsAsync([FromBody] LessonDtoForRegularLessons lessonDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _lessonService.AddRegularLessonsAsync(lessonDto);
                return Ok("Регулярные уроки успешно добавлены.");
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Ошибка при добавлении регулярных уроков: {e.Message}");
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAllAsync()
        {
            await _lessonService.DeleteAllAsync();
            return NoContent();
        }

        [HttpPost("auto-search")]
        public async Task<IActionResult> AutoSearchAsync([FromBody] LessonDtoForAutoAdd lessonDtoForAutoAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _lessonService.AutoSearch(lessonDtoForAutoAdd);
                return Ok("Регулярные уроки успешно добавлены на основе авто-поиска.");
            }
            catch (InvalidOperationException e)
            {
                return NotFound(e.Message); // например, "Нет свободных преподавателей для выбранного времени."
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Ошибка при автоматическом поиске и добавлении уроков: {e.Message}");
            }
        }

    }
}
