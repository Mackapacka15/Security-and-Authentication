using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecurityAndAuth.Models;

namespace SecurityAndAuth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager
) : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (model == null)
        {
            return BadRequest("Invalid user data.");
        }

        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        // Ensure role exists
        if (!await _roleManager.RoleExistsAsync(model.Role))
        {
            await _roleManager.CreateAsync(new IdentityRole(model.Role));
        }

        await _userManager.AddToRoleAsync(user, model.Role);

        return Ok(new { Message = "User registered successfully", UserId = user.Id });
    }
}
