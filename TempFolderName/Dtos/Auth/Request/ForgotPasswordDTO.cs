using System.ComponentModel.DataAnnotations;

namespace Shop.WebAPI.Dtos.Auth.Request;

public class ForgotPasswordDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
