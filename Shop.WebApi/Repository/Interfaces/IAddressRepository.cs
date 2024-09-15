using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Repository.Interfaces;

public interface IAddressRepository
{
    Task<Address> GetByIdAsync(int id);
    Task<IEnumerable<Address>> GetAllAsync();
    Task AddAsync(Address address);
    Task UpdateAsync(Address address);
    Task DeleteAsync(int id);
}