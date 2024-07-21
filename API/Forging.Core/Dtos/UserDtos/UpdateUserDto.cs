namespace Forging.Core.Dtos
{
    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public List<string> Email { get; set; }
        public List<string> PhoneNumber { get; set; }
        public List<string> Roles { get; set; }
        public string ImageUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
