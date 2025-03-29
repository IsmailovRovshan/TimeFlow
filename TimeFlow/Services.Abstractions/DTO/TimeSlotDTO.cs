
namespace Services.Abstractions.DTO
{
    public record TimeSlotDTO(Guid Id, DayOfWeek DayOfWeek, TimeSpan Time);
    
}
