using AutoMapper;
using Shop.WebAPI.Dtos.Size.Responses;
using Shop.WebAPI.Repository.Interfaces;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Services;

public class SizeService : ISizeService
{
    private readonly ISizeRepository _sizeRepository;
    private readonly IMapper _mapper; // Используем AutoMapper для маппинга сущностей и DTO

    public SizeService(ISizeRepository sizeRepository, IMapper mapper)
    {
        _sizeRepository = sizeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetSizeResponse>> GetAllSizesAsync()
    {
        var sizes = await _sizeRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<GetSizeResponse>>(sizes);
    }

    public async Task<GetSizeResponse> GetSizeByIdAsync(int id)
    {
        var size = await _sizeRepository.GetByIdAsync(id);
        return _mapper.Map<GetSizeResponse>(size);
    }
}
