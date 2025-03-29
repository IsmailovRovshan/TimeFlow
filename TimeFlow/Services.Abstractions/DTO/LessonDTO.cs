using Domain;

namespace Services.Abstractions.DTO
{
    public record LessonDto(Guid TeacherId, Guid ClientId, DateTime LessonDate, Status Status);
    public record LessonDtoForCreate(Guid TeacherId, Guid ClientId, DateTime LessonDate, Status Status);
    public record LessonDtoForAutoCreate(Guid ClientId, DateTime LessonDate, Status Status);
    public record LessonDtoForUpdate(DateTime LessonDate, Status Status);
}
