using Microsoft.AspNetCore.Identity;
using Shop.WebAPI.Dtos.Auth;
using Shop.WebAPI.Dtos.Auth.Request;
using Shop.WebAPI.Dtos.Auth.Response;

namespace Shop.WebAPI.Services.Interfaces;

public interface IJwtService
{
    Task<AuthResult> GenerateToken(IdentityUser user);
    Task<RefreshTokenResponseDTO> VerifyToken(TokenRequestDTO tokenRequest);
    
}