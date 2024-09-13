using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Register DbContext in Program.cs
builder.Services.AddDbContext<DataContext>(opt =>
{
    // get connection string from appsettings.json
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.


app.MapControllers();

app.Run();
