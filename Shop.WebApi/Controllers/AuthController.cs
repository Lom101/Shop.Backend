using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Auth;
using Shop.WebAPI.Dtos.Auth.Request;
using Shop.WebAPI.Dtos.Auth.Response;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // Identity package
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtService _jwtService;
    private readonly IEmailSender _emailSender;

    public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IJwtService jwtService, IEmailSender emailSender)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
        _emailSender = emailSender;
    }

    [HttpPost("registration")]
    public async Task<IActionResult> Registration(RegisterUserDTO user)
    {
        if (ModelState.IsValid)
        {
            IdentityUser existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser != null)
            {
                return BadRequest(new RegisterResponseDTO()
                {
                    Errors = new List<string>() { "Email already Registered" },
                    Success = false
                });
            }

            IdentityUser newUser = new IdentityUser()
            {
                Email = user.Email,
                UserName = user.Username,
            };

            IdentityResult? created = await _userManager.CreateAsync(newUser, user.Password);
            if (created.Succeeded)
            {
                // Assign the "User" role to the new user
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }

                await _userManager.AddToRoleAsync(newUser, "User");
                AuthResult authResult = await _jwtService.GenerateToken(newUser);
                // Return a token
                return Ok(authResult);
            }
            else
            {
                return BadRequest(new RegisterResponseDTO()
                {
                    Errors = created.Errors.Select(e => e.Description).ToList(),
                    Success = false
                });
            }
        }

        return BadRequest(new RegisterResponseDTO()
        {
            Errors = new List<string>() { "Invalid payload" },
            Success = false
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDTO user)
    {
        if (ModelState.IsValid)
        {
            IdentityUser existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser == null)
            {
                return BadRequest(new RegisterResponseDTO()
                {
                    Errors = new List<string>() { "Email address is not registered." },
                    Success = false
                });
            }

            bool isUserCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);
            if (isUserCorrect)
            {
                AuthResult authResult = await _jwtService.GenerateToken(existingUser);
                //return a token
                return Ok(authResult);
            }
            else
            {
                return BadRequest(new RegisterResponseDTO()
                {
                    Errors = new List<string>() { "Wrong password" },
                    Success = false
                });
            }
        }

        return BadRequest(new RegisterResponseDTO()
        {
            Errors = new List<string>() { "Invalid payload" },
            Success = false
        });
    }

    [HttpPost("refresh_token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDTO tokenRequest)
    {
        if (ModelState.IsValid)
        {
            var verified = await _jwtService.VerifyToken(tokenRequest);
            //
            if (!verified.Success)
            {
                return BadRequest(new AuthResult()
                {
                    // Errors = new List<string> { "invalid Token" },
                    Errors = verified.Errors,
                    Success = false
                });
            }

            var tokenUser = await _userManager.FindByIdAsync(verified.UserId);
            AuthResult authResult = await _jwtService.GenerateToken(tokenUser);
            // Return a token
            return Ok(authResult);
        }

        return BadRequest(new AuthResult()
        {
            Errors = new List<string> { "invalid Payload" },
            Success = false
        });
    }
    
    // Step 1: Generate and send the password reset token
    [HttpPost("forgot_password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Success = false, Errors = new List<string> { "Invalid model" } });
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return BadRequest(new { Success = false, Errors = new List<string> { "Email not registered" } });
        }

        // Генерируем код (например, 6 цифр)
        var resetCode = new Random().Next(100000, 999999).ToString();
        // Сохраняем код (например, в БД или временном хранилище)
        await _userManager.SetAuthenticationTokenAsync(user, "ResetPassword", "PasswordResetCode", resetCode);
        // Отправляем код на почту
        var emailContent = $"Your password reset code is {resetCode}";
        
        
        await _emailSender.SendEmailAsync(model.Email, "Password Reset", emailContent);

        return Ok(new { Success = true, Message = "Password reset link sent to email" });
    }
    
    // Step 2: Verify the token and reset the password
    [HttpPost("reset_password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Success = false, Errors = new List<string> { "Invalid model" } });
        }
        
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return BadRequest(new { Success = false, Errors = new List<string> { "Invalid email" } });
        }
        
        // Получаем сохраненный код
        var savedCode = await _userManager.GetAuthenticationTokenAsync(user, "ResetPassword", "PasswordResetCode");
        if (savedCode != model.Code)
            return BadRequest("Invalid reset code.");
        
        // создаем токен для смены пароля(типо костыль, а может и норм)
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        // Reset the user's password
        var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest(new { Success = false, Errors = result.Errors.Select(e => e.Description).ToList() });
        }

        return Ok(new { Success = true, Message = "Password has been reset successfully" });
    }
}
