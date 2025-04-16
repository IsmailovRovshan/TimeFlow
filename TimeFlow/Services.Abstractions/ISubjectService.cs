using Services.Abstractions.DTO;

namespace Services.Abstractions
{
    public interface ISubjectService
    {
        Task<SubjectDto> GetByIdAsync(Guid id);
        Task<List<SubjectDto>> GetAllAsync();
        Task<SubjectDto> CreateAsync(SubjectDtoForCreate subjectDto);
        Task UpdateAsync(Guid subjectId, SubjectDtoForUpdate subject);
        Task DeleteAsync(Guid subjectId);
        Task DeleteAllAsync();

    }
}
