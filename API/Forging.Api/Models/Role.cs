using System.ComponentModel;
using Dapper.Contrib.Extensions;

namespace Forging.Api.Models
{
    [Table("roles")]
    public class Role
    {
        [Key]
        [Description("id")]
        public Guid Id { get; set; }

        [Description("role_name")]
        public string RoleName { get; set; }
    }
}
