using Shop.WebAPI.Dtos.Brand;
using Shop.WebAPI.Dtos.Brand.Request;

namespace Shop.WebAPI.Services.Interfaces;

public interface IBrandService
{
    Task<IEnumerable<BrandDto>> GetAllBrandsAsync();
    Task<BrandDto> CreateBrandAsync(CreateBrandRequest request);
    Task<BrandDto> UpdateBrandAsync(UpdateBrandRequest request);
    Task<bool> DeleteBrandAsync(int id);
}