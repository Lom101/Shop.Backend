using Microsoft.AspNetCore.Identity;
using Shop.WebAPI.Dtos.Auth;
using Shop.WebAPI.Dtos.Auth.Request;
using Shop.WebAPI.Dtos.Auth.Response;
using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Services.Interfaces;

public interface IJwtService
{
    Task<AuthResult> GenerateToken(ApplicationUser user);
    Task<RefreshTokenResponseDTO> VerifyToken(TokenRequestDTO tokenRequest);
    
}