using Domain.Enums;
using Services.Abstractions.DTO;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface ILessonService
    {
        Task<LessonDto> GetByIdAsync(Guid TeacherId, Guid ClientId);
        Task<List<LessonDto>> GetAllAsync();
        Task<List<LessonDto>> GetAllByClientId(Guid ClientId);
        
        Task DeleteAllAsync();
        Task<LessonDto> CreateAsync(LessonDtoForCreate lessonDto);
        Task UpdateAsync(Guid TeacherId, Guid ClientId, LessonDtoForUpdate lessonDto);
        Task DeleteAsync(Guid TeacherId, Guid ClientId);

        Task<List<LessonDto>> GetLessonsByDateAsync(Guid teacherId, DateTime date);
        Task<List<LessonDto>> GetLessonsInRangeAsync(LessonDtoInRange lessonDto);
        
        Task<UserDto> AddRegularLessonsAsync(CreateRegularLessonsDto lessonDto);
        
        Task<List<LessonDto>> GetAllByClientIdAndDateAsync(Guid clientId, DateTime date);

        Task<UserDto> MainCreateLesson(MainCreateLessonDto lessonDtos, Guid UserId);
        Task<UserDto> MainCreateLesson(MainCreateLessonDto lessonDtos, SearchMode mode);
        
        Task DeleteLessonByIdAsync(Guid lessonId);
        Task<LessonDto> RescheduleLessonAsync(RescheduleLessonDto dto);

    }
}
