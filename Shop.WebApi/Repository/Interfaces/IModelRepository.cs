using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Repository.Interfaces;

public interface IModelRepository
{
    Task<Model> GetByIdAsync(int id);
    Task<IEnumerable<Model>> GetAllAsync();
    Task AddAsync(Model model);
    Task UpdateAsync(Model model);
    Task DeleteAsync(Model model);
}