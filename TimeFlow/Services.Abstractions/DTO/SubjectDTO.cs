
namespace Services.Abstractions.DTO
{
    public record SubjectDto(Guid Id, string? Name);
    public record SubjectDtoForCreate(string Name);
    public record SubjectDtoForUpdate(string Name);
}
