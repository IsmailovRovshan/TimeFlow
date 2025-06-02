namespace Domain.Entities
{
    public class Lesson
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid ClientId { get; set; }
        public Client? Client { get; set; }
        
        public Guid SubjectId { get; set; }
        public Subject? Subject { get; set; }

        public DateTime LessonDate { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Запланирован;
    }
}
