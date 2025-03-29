namespace Domain.Entities
{
    public class TimeSlot
    {
        public Guid Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan Time { get; set; }

        public List<Teacher> Teachers { get; set; } = new List<Teacher>();
    }
}
