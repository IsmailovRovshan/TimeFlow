namespace Services.Abstractions.DTO
{
    public record ClientDto(Guid Id, string FullName, int Age);
    public record ClientDtoForCreate(string FullName, int Age);
    public record ClientDtoForUpdate(string FullName, int Age);
}
