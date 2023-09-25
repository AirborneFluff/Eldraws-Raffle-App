using Azure_WebApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Azure_WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController: ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        
        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("test")]
        public async Task<ActionResult<string>> Test()
        {
            var result = await _userManager.Users.FirstOrDefaultAsync();
            if (result == null) return NotFound("No users in the database");

            return Ok($"Username: {result.UserName}");
        }
    }
}