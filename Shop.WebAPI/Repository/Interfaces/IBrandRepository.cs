using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Repository.Interfaces;

public interface IBrandRepository
{
    Task<IEnumerable<Brand>> GetAllAsync();
    Task<Brand> GetByIdAsync(int id);
    Task<bool> AddAsync(Brand brand);
    Task<bool> UpdateAsync(Brand brand);
    Task<bool> DeleteAsync(int id);
}