using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Repository.Interfaces;

public interface IColorRepository
{
    Task<IEnumerable<Color>> GetAllAsync();
    Task<Color> GetByIdAsync(int id);
}
