using System.Data;
using Dapper;
using Microsoft.IdentityModel.Tokens;

namespace Forging.Api.Handlers
{
    public class StringListTypeHandler : SqlMapper.TypeHandler<List<string>>
    {
        public override void SetValue(IDbDataParameter parameter, List<string> value)
        {
            parameter.Value = string.Join(",", value);
        }

        public override List<string> Parse(object value)
        {
            value ??= string.Empty;
            var emails = (value as string)!.Replace("{", "").Replace("}", "").Replace("\"", "");
            return emails.IsNullOrEmpty() ? new List<string>() : emails.Split(',').ToList();
        }
    }
}
