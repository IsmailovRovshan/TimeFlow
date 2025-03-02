namespace Domain.Entities
{
    public class Client
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public Guid ManagerId { get; set; }
        public Manager? Manager { get; set; }

        public List<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}
