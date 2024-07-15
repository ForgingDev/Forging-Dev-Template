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

        [Description("name")]
        public string Name { get; set; }
    }
}
