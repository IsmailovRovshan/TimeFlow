
namespace Services.Abstractions.DTO
{
    public record TimeSlotDto(Guid Id, DayOfWeek DayOfWeek, TimeSpan Time, bool IsBusy, Guid UserId);
    public record TimeSlotDtoForCreate(DayOfWeek DayOfWeek, TimeSpan Time, bool IsBusy, Guid UserId);
    public record TimeSlotDtoForUpdate(DayOfWeek DayOfWeek, TimeSpan Time, Guid UserId);

    public record TimeSlotFilterDto(DayOfWeek DayOfWeek, TimeSpan Time);

}
