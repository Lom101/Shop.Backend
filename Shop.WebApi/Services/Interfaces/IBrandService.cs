using Shop.WebAPI.Dtos.Brand;
using Shop.WebAPI.Dtos.Brand.Request;
using Shop.WebAPI.Dtos.Brand.Response;

namespace Shop.WebAPI.Services.Interfaces;

public interface IBrandService
{
    Task<IEnumerable<GetBrandResponse>> GetAllBrandsAsync();
    Task<GetBrandResponse> GetBrandByIdAsync(int id);
    Task<GetBrandResponse> AddBrandAsync(CreateBrandRequest request);
    Task<GetBrandResponse> UpdateBrandAsync(UpdateBrandRequest request);
    Task<bool> DeleteBrandAsync(int id);
}