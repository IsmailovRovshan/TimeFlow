using Services.Abstractions.DTO;

namespace Services.Abstractions
{
    public interface ILessonService
    {
        Task<LessonDto> GetByIdAsync(Guid TeacherId, Guid ClientId);
        Task<List<LessonDto>> GetAllAsync();
        Task DeleteAllAsync();
        Task<LessonDto> CreateAsync(LessonDtoForCreate lessonDto);
        Task UpdateAsync(Guid TeacherId, Guid ClientId, LessonDtoForUpdate lessonDto);
        Task DeleteAsync(Guid TeacherId, Guid ClientId);

        Task<List<LessonDto>> GetLessonsByDateAsync(Guid teacherId, DateTime date);
        Task<List<LessonDto>> GetLessonsInRangeAsync(LessonDtoInRange lessonDto);
        
        Task AddRegularLessonsAsync(LessonDtoForRegularLessons lessonDto);
        Task<UserDto> AutoSearch(LessonDtoForAutoAdd lessonDtoForAutoAdd);

    }
}
