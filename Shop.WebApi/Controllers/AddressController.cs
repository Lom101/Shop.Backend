using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Address.Requests;
using Shop.WebAPI.Dtos.Address.Responses;
using Shop.WebAPI.Repository.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly IAddressRepository _addressRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public AddressController(
        IAddressRepository addressRepository, 
        UserManager<ApplicationUser> userManager, 
        IMapper mapper)
    {
        _addressRepository = addressRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    // GET: api/address/user/{userId}
    [HttpGet("user/{userId}")]
    [Authorize(Roles = "Admin")] 
    public async Task<IActionResult> GetAddressesByUserId(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("Пользователь не найден.");
        }

        var addresses = await _addressRepository.GetByUserIdAsync(userId);
        if (!addresses.Any())
        {
            return NotFound("Адреса для пользователя не найдены.");
        }

        var mappedAddresses = _mapper.Map<IEnumerable<GetAddressResponse>>(addresses);
        return Ok(mappedAddresses);
    }

    // GET: api/address/me  
    [HttpGet("me")]
    [Authorize(Roles = "User,Admin")] 
    public async Task<IActionResult> GetMyAddresses()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Пользователь не авторизован.");
        }

        var addresses = await _addressRepository.GetByUserIdAsync(userId);
        if (addresses == null || !addresses.Any())
        {
            return NotFound("Для текущего пользователя адреса не найдены.");
        }

        var addressResponses = _mapper.Map<IEnumerable<GetAddressResponse>>(addresses);
        return Ok(addressResponses);
    }

}