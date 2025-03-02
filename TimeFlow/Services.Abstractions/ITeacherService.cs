using Services.Abstractions.DTO;

namespace Services.Abstractions
{
    public interface ITeacherService
    {
        Task<TeacherDto> GetByIdAsync(Guid id);
        Task<List<TeacherDto>> GetAllAsync();
        Task<TeacherDto> CreateAsync(TeacherDtoForCreate teacherDto);
        Task UpdateAsync(Guid teacherId, TeacherDtoForUpdate teacher);
        Task DeleteAsync(Guid teacherId);
    }
}
