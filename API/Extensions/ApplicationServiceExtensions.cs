using System.Text;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
  {
    services.AddControllers();
    services.AddDbContext<DataContext>(opt =>
    {
      opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
    });
    services.AddCors();
    services.AddScoped<ITokenService, TokenService>();
    return services;
  }

  public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
  {
    services
      .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
        var tokenKey = config["TokenKey"] ?? throw new Exception("Token key not found");
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });
    return services;
  }
}


/*
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt =>
{
  opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();
builder.Services.AddScoped<ITokenService, TokenService>();
*/

/*
  builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
    var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("Token key not found");
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
      ValidateIssuer = false,
      ValidateAudience = false
    };
  });
*/