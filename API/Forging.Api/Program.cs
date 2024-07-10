using System.ComponentModel;
using System.Reflection;
using API.Handlers;
using API.Models;
using Dapper;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var connectionString = @$"Host={builder.Configuration["DATABASE_HOST_SUPABASE"]};
                          Port={builder.Configuration["DATABASE_PORT_SUPABASE"]};
                          Database={builder.Configuration["DEFAULT_DATABASE_NAME"]};
                          User Id={builder.Configuration["DATABASE_USERNAME_SUPABASE"]};
                          Password={builder.Configuration["DATABASE_PASSWORD_SUPABASE"]};";
builder.Services.AddSingleton<NpgsqlConnection>(_ => new NpgsqlConnection(connectionString));

var map = new CustomPropertyTypeMap(
    typeof(User),
    (type, columnName) =>
        type.GetProperties().FirstOrDefault(prop => GetDescriptionFromAttribute(prop) == columnName)
);

static string GetDescriptionFromAttribute(MemberInfo member)
{
    if (member == null)
        return null;

    var attrib = (DescriptionAttribute)
        Attribute.GetCustomAttribute(member, typeof(DescriptionAttribute), false);

    return attrib == null ? member.Name : attrib.Description;
}

SqlMapper.SetTypeMap(typeof(User), map);

SqlMapper.AddTypeHandler(new StringListTypeHandler());
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.MapControllers();

app.Run();