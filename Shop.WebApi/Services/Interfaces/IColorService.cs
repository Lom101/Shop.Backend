using Shop.WebAPI.Dtos.Color.Responses;

namespace Shop.WebAPI.Services.Interfaces;

public interface IColorService
{
    Task<IEnumerable<GetColorResponse>> GetAllColorsAsync();
    Task<GetColorResponse> GetColorByIdAsync(int id);
}
