using System.Text;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// extracted into extension method
builder.Services.AddApplicationServices(builder.Configuration);
// adding auth configurations
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();


//CORS middleware
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));


// auth middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
