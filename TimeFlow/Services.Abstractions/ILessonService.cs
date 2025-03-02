using Services.Abstractions.DTO;

namespace Services.Abstractions
{
    public interface ILessonService
    {
        Task<LessonDto> GetByIdAsync(Guid TeacherId, Guid ClientId);
        Task<List<LessonDto>> GetAllAsync();
        Task<LessonDto> CreateAsync(LessonDtoForCreate lessonDto);
        Task UpdateAsync(Guid TeacherId, Guid ClientId, LessonDtoForUpdate lessonDto);
        Task DeleteAsync(Guid TeacherId, Guid ClientId);

    }
}
