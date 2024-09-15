using AutoMapper;
using Shop.WebAPI.Dtos.Address.Requests;
using Shop.WebAPI.Dtos.Address.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper; // AutoMapper

    public AddressService(IAddressRepository addressRepository, IMapper mapper)
    {
        _addressRepository = addressRepository;
        _mapper = mapper;
    }
    public async Task<GetAddressResponse> GetAddressByIdAsync(int id)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        return _mapper.Map<GetAddressResponse>(address);
    }

    public async Task<IEnumerable<GetAddressResponse>> GetAllAddressesAsync()
    {
        var addresses = await _addressRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<GetAddressResponse>>(addresses);
    }

    public async Task<int> AddAddressAsync(CreateAddressRequest addressDto)
    {
        var address = _mapper.Map<Address>(addressDto);
        await _addressRepository.AddAsync(address);
        return address.Id;
    }

    public async Task<bool> UpdateAddressAsync(UpdateAddressRequest addressDto)
    {
        var address = _mapper.Map<Address>(addressDto);
        var existingAddress = await _addressRepository.GetByIdAsync(address.Id);
        if (existingAddress == null)
        {
            return false; // Адрес не найден
        }
        await _addressRepository.UpdateAsync(address);
        return true;
    }

    public async Task<bool> DeleteAddressAsync(int id)
    {
        var existingAddress = await _addressRepository.GetByIdAsync(id);
        if (existingAddress == null)
        {
            return false; // Адрес не найден
        }
        await _addressRepository.DeleteAsync(id);
        return true;
    }
}