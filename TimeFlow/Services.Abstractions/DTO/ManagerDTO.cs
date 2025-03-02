namespace Services.Abstractions.DTO
{
    public record ManagerDto(Guid Id, string Login, string Password, string Email, string FullName);
    public record ManagerDtoForCreate(string Login, string Password, string FullName, string Email);
    public record ManagerDtoForUpdate(string Email);
}
