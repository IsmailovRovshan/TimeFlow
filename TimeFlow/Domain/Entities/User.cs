namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Role Role { get; set; } 
        
        public Guid MangerId { get; set; }
        public Manager? Manager { get; set; }

        public Guid TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
    }
    public enum Role {
        Teacher = 0,
        Manager = 1
    }
}
