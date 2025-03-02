namespace Domain.Entities
{
    public class TimeSlot
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
    }
}
