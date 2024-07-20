using Forging.Api.Extensions;
using System.ComponentModel;
using System.Reflection;
using Forging.Api.Handlers;
using Forging.Domain.Models;
using Dapper;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);

var connectionString =
    @$"Host={builder.Configuration["DATABASE_HOST_SUPABASE"]};
    Port={builder.Configuration["DATABASE_PORT_SUPABASE"]};
    Database={builder.Configuration["DEFAULT_DATABASE_NAME"]};
    User Id={builder.Configuration["DATABASE_USERNAME_SUPABASE"]};
    Password={builder.Configuration["DATABASE_PASSWORD_SUPABASE"]};";
builder.Services.AddTransient<NpgsqlConnection>(_ => new NpgsqlConnection(connectionString));

//TODO make this method abstract and extract it into extesions/helpers

var userMap = new CustomPropertyTypeMap(
    typeof(User),
    (type, columnName) =>
        type.GetProperties().FirstOrDefault(prop => GetDescriptionFromAttribute(prop) == columnName)
);

var roleMap = new CustomPropertyTypeMap(
    typeof(Role),
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

SqlMapper.SetTypeMap(typeof(User), userMap);
SqlMapper.SetTypeMap(typeof(Role), roleMap);

SqlMapper.AddTypeHandler(new StringListTypeHandler());
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
