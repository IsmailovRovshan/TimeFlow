using Domain;
using Domain.Entities;

namespace Services.Abstractions.DTO
{
    public record LessonDto(Guid Id, Guid UserId, Guid ClientId, ClientDto Client,  DateTime LessonDate, Status Status, Guid SubjectId, SubjectDto Subject);
    public record LessonDtoForCreate(Guid UserId, Guid ClientId, DateTime LessonDate, Status Status, Guid SubjectId);

    public record LessonDtoForUpdate(DateTime LessonDate, Status Status);
    public record LessonDtoInRange(Guid UserId, DateTime startDate, DateTime endDate);

    public record LessonDtoForAutoAdd(Guid ClientId, DayOfWeek DayOfWeek, TimeSpan Time, int Number);

    public record CreateRegularLessonsDto(Guid UserId, Guid ClientId,ClientDto Client, List<TimeSlotDtoDateWithTime> Slots, DateTime StartDate, int Number, Guid SubjectId, SubjectDto? Subject);

    public record MainCreateLessonDto(string FullName, int Age, Guid SubjectId, List<TimeSlotDtoDateWithTime> Slots, 
        DateTime StartDate, int Number);
    public record RescheduleLessonDto(
        Guid LessonId,           
        DateTime NewLessonDate   
    );
}
