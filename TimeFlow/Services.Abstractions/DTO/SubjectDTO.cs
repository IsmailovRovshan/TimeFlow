
namespace Services.Abstractions.DTO
{
    public record SubjectDto(Guid Id, string? Name, List<UserDto>? Users);
    public record SubjectDtoForCreate(string Name, List<UserDto>? Users);
    public record SubjectDtoForUpdate(string Name, List<UserDto> Users);
}
