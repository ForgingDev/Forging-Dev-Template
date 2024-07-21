using System.ComponentModel;
using Dapper.Contrib.Extensions;

namespace Forging.Domain.Models
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
