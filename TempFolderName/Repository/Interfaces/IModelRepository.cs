using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Repository.Interfaces;

public interface IModelRepository
{
    Task<Model> GetModelByIdAsync(int id);
    Task<IEnumerable<Model>> GetAllModelsAsync();
    Task<int> AddModelAsync(Model model);
    Task<bool> UpdateModelAsync(Model model);
    Task<bool> DeleteModelAsync(int id);
}