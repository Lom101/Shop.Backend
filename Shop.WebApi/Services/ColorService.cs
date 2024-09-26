using AutoMapper;
using Shop.WebAPI.Dtos.Color.Responses;
using Shop.WebAPI.Repository.Interfaces;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Services;

public class ColorService : IColorService
{
    private readonly IColorRepository _colorRepository;
    private readonly IMapper _mapper;

    public ColorService(IColorRepository colorRepository, IMapper mapper)
    {
        _colorRepository = colorRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetColorResponse>> GetAllColorsAsync()
    {
        var colors = await _colorRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<GetColorResponse>>(colors);
    }

    public async Task<GetColorResponse> GetColorByIdAsync(int id)
    {
        var color = await _colorRepository.GetByIdAsync(id);
        return _mapper.Map<GetColorResponse>(color);
    }
}
