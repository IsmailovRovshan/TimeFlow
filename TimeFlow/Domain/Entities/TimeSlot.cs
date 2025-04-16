namespace Domain.Entities
{
    public class TimeSlot
    {
        public Guid Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan Time { get; set; }
        public bool IsBusy { get; set; } = true;

        public User? User { get; set; } 
        public Guid UserId { get; set; } 
    }
}
