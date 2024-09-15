using Shop.WebAPI.Dtos.Address.Requests;
using Shop.WebAPI.Dtos.Address.Responses;

namespace Shop.WebAPI.Services.Interfaces;

public interface IAddressService
{
    Task<GetAddressResponse> GetAddressByIdAsync(int id);
    Task<IEnumerable<GetAddressResponse>> GetAllAddressesAsync();
    Task<int> AddAddressAsync(CreateAddressRequest addressDto);
    Task<bool> UpdateAddressAsync(UpdateAddressRequest addressDto);
    Task<bool> DeleteAddressAsync(int id);
}