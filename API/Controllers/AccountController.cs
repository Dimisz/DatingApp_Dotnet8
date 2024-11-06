using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext ctx, ITokenService tokenService) : BaseApiController
{
  [HttpPost("register")] // account/register
  public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
  {
    if (await UserExists(registerDTO.Username)) return BadRequest("username is taken");

    using var hmac = new HMACSHA512();
    var user = new AppUser
    {
      UserName = registerDTO.Username.ToLower(),
      PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
      PasswordSalt = hmac.Key
    };

    ctx.Users.Add(user);
    await ctx.SaveChangesAsync();
    return new UserDTO
    {
      Username = user.UserName,
      Token = tokenService.CreateToken(user)
    };
  }

  [HttpPost("login")]
  public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
  {
    var user = await ctx.Users.FirstOrDefaultAsync(x => x.UserName == loginDTO.Username.ToLower());

    // BEING SPECIFIC HERE FOR DEV PURPOSES ONLY
    // SHOULD BE CHANGED TO 'INVALID CREDENTIALS' IN PROD
    if (user == null) return Unauthorized("Invalid username");

    using var hmac = new HMACSHA512(user.PasswordSalt);
    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
    for (int i = 0; i < computedHash.Length; i++)
    {
      // BEING SPECIFIC HERE FOR DEV PURPOSES ONLY
      // SHOULD BE CHANGED TO 'INVALID CREDENTIALS' IN PROD
      if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
    }
    return new UserDTO
    {
      Username = user.UserName,
      Token = tokenService.CreateToken(user)
    };
  }


  private async Task<bool> UserExists(string username)
  {
    return await ctx.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
  }
}
