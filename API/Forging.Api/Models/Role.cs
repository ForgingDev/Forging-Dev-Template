using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forging.Api.Models
{
    [Table("roles")]
    public class Role
    {
        [Key]
        public Guid Id { get; set; }

        [Description("role_name")]
        public string RoleName { get; set; }
    }
}
