using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


public class UsersController(DataContext ctx) : BaseApiController
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
