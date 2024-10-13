using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(UserManager<IdentityUser> userManager, ILogger<ProfileController> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (userIdClaim == null || emailClaim == null)
        {
            return NotFound("User claims not found");
        }

        var model = new
        {
            UserId = userIdClaim,
            Email = emailClaim
        };
        return Ok(model);
    }
}