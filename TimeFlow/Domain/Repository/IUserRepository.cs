using Domain.Entities;

namespace Domain.Repository
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<List<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task DeleteAllAsync();

        Task<List<User>> GetByIdsAsync(List<Guid> UserIds);
        Task<User?> GetByLoginAsync(string login);

        Task<List<User>> GetFreeAsync(IEnumerable<TimeSlot> requestedSlots);


        Task AddSubjectToUserAsync(User user, Subject subject);
        Task RemoveSubjectFromUserAsync(User user, Subject subject);
    }
}
