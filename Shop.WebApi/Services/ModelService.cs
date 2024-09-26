using AutoMapper;
using Shop.WebAPI.Dtos.Model.Request;
using Shop.WebAPI.Dtos.Model.Response;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Services;

public class ModelService : IModelService
{
    private readonly IModelRepository _modelRepository;
    private readonly IMapper _mapper;

    public ModelService(IModelRepository modelRepository, IMapper mapper)
    {
        _modelRepository = modelRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetModelResponse>> GetAllModelsAsync()
    {
        var models = await _modelRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<GetModelResponse>>(models);
    }

    public async Task<GetModelResponse> GetModelByIdAsync(int id)
    {
        var model = await _modelRepository.GetByIdAsync(id);
        return _mapper.Map<GetModelResponse>(model);
    }

    public async Task<int> AddModelAsync(CreateModelRequest request)
    {
        var model = _mapper.Map<Model>(request);
        await _modelRepository.AddAsync(model);
        return model.Id;
    }

    public async Task<bool> UpdateModelAsync(UpdateModelRequest request)
    {
        var model = await _modelRepository.GetByIdAsync(request.Id);
        if (model == null) return false;

        _mapper.Map(request, model);
        await _modelRepository.UpdateAsync(model);
        return true;
    }

    public async Task<bool> DeleteModelAsync(int id)
    {
        var model = await _modelRepository.GetByIdAsync(id);
        if (model == null) return false;

        await _modelRepository.DeleteAsync(model);
        return true;
    }
}
