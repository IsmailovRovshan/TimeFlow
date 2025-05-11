using Domain.Enums;
using Services.Abstractions.DTO;
using System.Threading.Tasks;

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
        
        Task<UserDto> AddRegularLessonsAsync(CreateRegularLessonsDto lessonDto);
        //Task<UserDto> AutoSearch(LessonDtoForAutoAdd lessonDtoForAutoAdd);

        //Task<UserDto> AutoSearchMulti(IEnumerable<LessonDtoForAutoAdd> lessonDtos);

        Task<UserDto> MainCreateLesson(MainCreateLessonDto lessonDtos, Guid UserId);
        Task<UserDto> MainCreateLesson(MainCreateLessonDto lessonDtos, SearchMode mode);

    }
}
