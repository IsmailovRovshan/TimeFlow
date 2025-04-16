
namespace Domain.Entities
{
    public class Subject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<User> Users { get; set; } = new List<User>();

    }
}
