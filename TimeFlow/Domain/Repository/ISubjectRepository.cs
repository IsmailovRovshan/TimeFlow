using Domain.Entities;

namespace Domain.Repository
{
    public interface ISubjectRepository
    {
        Task<Subject> GetByIdAsync(Guid id);
        Task<List<Subject>> GetAllAsync();
        Task AddAsync(Subject subject);
        Task UpdateAsync(Subject subject);
        Task DeleteAsync(Subject subject);

        Task DeleteAllAsync();
    }
}
