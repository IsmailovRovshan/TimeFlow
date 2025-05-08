
using Domain.Entities;
using Services.Abstractions.DTO;

namespace Services.Abstractions
{

    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto> CreateAsync(UserDtoForCreate userDto);
        Task UpdateAsync(Guid userId, UserDtoForUpdate userDto);
        Task DeleteAsync(Guid userId);
        Task DeleteAllAsync();

        //Task<List<UserDto>> GetFreeAsync(TimeSlotFilterDto timeSlotDto);
        //Task<UserDto> GetFreeTeacher(TimeSlotFilterDto timeSlotDto);

        Task<List<UserDto>> GetFreeAsync(IEnumerable<TimeSlotDtoDateWithTime> requestedSlots);
        //Task<UserDto> GetFreeTeacher(IEnumerable<TimeSlotFilterDto> requestedSlots);

        Task AddSubjectToUserAsync(Guid userId, Guid subjectId);
        Task RemoveSubjectFromUserAsync(Guid userId, Guid subjectId);

    }

}
