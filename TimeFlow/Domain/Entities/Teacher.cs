using System.Collections.Generic;

namespace Domain.Entities
{
    public class Teacher 
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        public List<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
        public List<Lesson> Lessons { get; set; } = new List<Lesson>();

        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
