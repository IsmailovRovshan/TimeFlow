namespace Domain.Entities
{
    public class Lesson
    {
        public Guid TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

        public Guid ClientId { get; set; }
        public Client? Client { get; set; }

        public DateTime LessonDate { get; set; } = DateTime.Now;
        //public Status Status { get; set; } = Status.Scheduled;
    }
}
