using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Repository.Interfaces;

public interface IAddressRepository
{
    Task<IEnumerable<Address>> GetByUserIdAsync(string userId);
}