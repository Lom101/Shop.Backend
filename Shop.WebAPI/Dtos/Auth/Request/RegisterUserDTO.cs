using System.ComponentModel.DataAnnotations;

namespace Shop.WebAPI.Dtos.Auth.Request;

public class RegisterUserDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
