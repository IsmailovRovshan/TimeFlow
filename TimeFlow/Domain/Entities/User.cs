namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? Age { get; set; }
        public int? Experiense { get; set; }
        public Role Role { get; set; }


        // Teacher
        public List<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
        public List<Lesson> Lessons { get; set; } = new List<Lesson>();

        public List<Subject> Subjects { get; set; } = new List<Subject>();
    }
    public enum Role {
        Teacher = 0,
        Manager = 1,
    }
}
