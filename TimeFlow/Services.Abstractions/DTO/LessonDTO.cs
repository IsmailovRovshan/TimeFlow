using Domain;
using Domain.Entities;

namespace Services.Abstractions.DTO
{
    public record LessonDto(Guid UserId, Guid ClientId, ClientDto Client,  DateTime LessonDate, Status Status);
    public record LessonDtoForCreate(Guid UserId, Guid ClientId, DateTime LessonDate, Status Status);
    public record LessonDtoForAutoCreate(Guid ClientId, DateTime LessonDate, Status Status);

    public record LessonDtoForUpdate(DateTime LessonDate, Status Status);
    public record LessonDtoInRange(Guid UserId, DateTime startDate, DateTime endDate);

    public record LessonDtoForAutoAdd(Guid ClientId, DayOfWeek DayOfWeek, TimeSpan Time, int Number);


    public record CreateRegularLessonsDto(Guid UserId, Guid ClientId, List<TimeSlotDtoDateWithTime> Slots, DateTime StartDate, int Number);
}
