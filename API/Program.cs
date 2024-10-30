using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt =>
{
  opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// configure DefaultConnection in appsettings
// then create migrations:
// dotnet ef migrations add InitialCreate -o Data/Migrations

// then
// generate db
// dotnet ef database update

var app = builder.Build();

app.MapControllers();

app.Run();
