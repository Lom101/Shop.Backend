using Shop.WebAPI.Dtos.Size.Responses;

namespace Shop.WebAPI.Services.Interfaces;

public interface ISizeService
{
    Task<IEnumerable<GetSizeResponse>> GetAllSizesAsync();
    Task<GetSizeResponse> GetSizeByIdAsync(int id);
}
