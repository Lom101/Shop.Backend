using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Repository.Interfaces;

public interface ISizeRepository
{
    Task<IEnumerable<Size>> GetAllSizesAsync();
    Task<Size> GetSizeByIdAsync(int id);
}
