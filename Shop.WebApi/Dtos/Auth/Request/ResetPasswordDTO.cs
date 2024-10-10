using System.ComponentModel.DataAnnotations;

namespace Shop.WebAPI.Dtos.Auth.Request;

public class ResetPasswordDTO
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; }
}
