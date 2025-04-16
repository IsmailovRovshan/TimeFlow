using Domain.Entities;

namespace Domain.Repository
{
    public interface ILessonRepository
    {
        Task<Lesson> GetByIdAsync(Guid UserId, Guid ClientId);
        Task<List<Lesson>> GetAllAsync();
        Task AddAsync(Lesson lesson);
        Task UpdateAsync(Lesson lesson);
        Task DeleteAsync(Lesson lesson);

        Task DeleteAllAsync();

        Task<List<Lesson>> GetLessonsByDateAsync(Guid teacherId, DateTime date);
        Task<List<Lesson>> GetLessonsInRangeAsync(Guid UserId, DateTime startDate, DateTime endDate);


        Task AddRegularLessonsAsync(DayOfWeek DayOfWeek, TimeSpan Time, int Number);
    }
}
