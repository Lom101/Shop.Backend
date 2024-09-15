using Microsoft.AspNetCore.Mvc;
using Shop.WebAPI.Dtos.Address.Requests;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    // GET: api/address
    [HttpGet]
    public async Task<IActionResult> GetAllAddresses()
    {
        var addresses = await _addressService.GetAllAddressesAsync();
        return Ok(addresses);
    }

    // GET: api/address/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAddressById(int id)
    {
        var address = await _addressService.GetAddressByIdAsync(id);
        if (address == null)
        {
            return NotFound();
        }
        return Ok(address);
    }

    // POST: api/address
    [HttpPost]
    public async Task<IActionResult> AddAddress([FromBody] CreateAddressRequest createAddressRequest)
    {
        var newAddressId = await _addressService.AddAddressAsync(createAddressRequest);
        return CreatedAtAction(nameof(GetAddressById), new { id = newAddressId }, createAddressRequest);
    }

    // PUT: api/address/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAddress(int id, [FromBody] UpdateAddressRequest updateAddressRequest)
    {
        if (id != updateAddressRequest.Id)
        {
            return BadRequest("address ID mismatch");
        }

        var isUpdated = await _addressService.UpdateAddressAsync(updateAddressRequest);
        if (!isUpdated)
        {
            return NotFound();
        }
        return NoContent();
    }

    // DELETE: api/address/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAddress(int id)
    {
        var isDeleted = await _addressService.DeleteAddressAsync(id);
        if (!isDeleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}