
using Domain.Entities;
using Services.Abstractions.DTO;

namespace Services.Abstractions
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto> CreateAsync(UserDtoForCreate userDto);
        Task UpdateAsync(Guid userId, UserDtoForUpdate user);
        Task DeleteAsync(Guid userId);
        Task DeleteAllAsync();

        // Поиск свободного преподавателя
        Task<List<UserDto>> GetFreeAsync(TimeSlotFilterDto timeSlotDto);
        Task<UserDto> GetFreeTeacher(TimeSlotFilterDto timeSlotDto);
    }
}
