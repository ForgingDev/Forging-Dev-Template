namespace API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public List<string> Email { get; set; }
        public List<string> PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
