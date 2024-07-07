namespace API.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public DateTime JoinedAt { get; set; }
    }
}
