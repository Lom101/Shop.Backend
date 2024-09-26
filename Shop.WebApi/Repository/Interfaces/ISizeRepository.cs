using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Repository.Interfaces;

public interface ISizeRepository
{
    Task<IEnumerable<Size>> GetAllAsync();
    Task<Size> GetByIdAsync(int id);
}
