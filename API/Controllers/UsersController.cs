using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] // localhost:5001/api/users
public class UsersController(DataContext ctx) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
  {
    var users = await ctx.Users.ToListAsync();

    return users;
  }

  [HttpGet("{id:int}")] // /api/users/1
  public async Task<ActionResult<AppUser>> GetUser(int id)
  {
    var user = await ctx.Users.FindAsync(id);

    if (user == null) return NotFound();
    return user;
  }
}
