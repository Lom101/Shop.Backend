using Shop.WebAPI.Dtos.Brand;
using Shop.WebAPI.Dtos.Brand.Request;
using Shop.WebAPI.Dtos.Brand.Response;

namespace Shop.WebAPI.Services.Interfaces;

public interface IBrandService
{
    Task<IEnumerable<BrandDto>> GetAllBrandsAsync();
    Task<GetBrandResponse> GetBrandByIdAsync(int id);
    Task<BrandDto> CreateBrandAsync(CreateBrandRequest request);
    Task<BrandDto> UpdateBrandAsync(UpdateBrandRequest request);
    Task<bool> DeleteBrandAsync(int id);
}