using Shop.WebAPI.Dtos.Model.Request;
using Shop.WebAPI.Dtos.Model.Response;

namespace Shop.WebAPI.Services.Interfaces;

public interface IModelService
{
    Task<IEnumerable<GetModelResponse>> GetAllModelsAsync();
    Task<GetModelResponse> GetModelByIdAsync(int id);
    Task<int> AddModelAsync(CreateModelRequest request);
    Task<bool> UpdateModelAsync(UpdateModelRequest request);
    Task<bool> DeleteModelAsync(int id);
}
