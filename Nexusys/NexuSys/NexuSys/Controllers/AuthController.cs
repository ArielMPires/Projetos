using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NexuSys.Data;
using NexuSys.Interfaces;
using System;
using System.Security.Claims;
namespace NexuSys.Controllers
{

    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] int ID, [FromForm] string password)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.ID == ID);

            if (user == null)
                return Unauthorized();

            // aqui entra seu hash
            if (user.Password != password)
                return Unauthorized();

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
        };

            var identity = new ClaimsIdentity(claims, "InternalAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("InternalAuth", principal);

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("InternalAuth");
            return Ok();
        }
    }
}
