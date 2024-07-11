using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forging.Api.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        public required string Id { get; set; }

        [Description("username")]
        public string? Username { get; set; }

        [Description("email")]
        public List<string> Email { get; set; }

        [Description("phone_number")]
        public List<string> PhoneNumber { get; set; }

        [Description("first_name")]
        public string FirstName { get; set; }

        [Description("last_name")]
        public string LastName { get; set; }

        [Description("image_url")]
        public string ImageUrl { get; set; }

        [Description("joined_at")]
        public DateTime JoinedAt { get; set; }
    }
}
