using Domain.Entities;

namespace Domain.Repository
{
    public interface ILessonRepository
    {
        Task<Lesson> GetByIdAsync(Guid TeacherId, Guid ClientId);
        Task<List<Lesson>> GetAllAsync();
        Task AddAsync(Lesson lesson);
        Task UpdateAsync(Lesson lesson);
        Task DeleteAsync(Lesson lesson);


        Task<List<Lesson>> GetLessonsByDateAsync(Guid teacherId, DateTime date);

    }
}
