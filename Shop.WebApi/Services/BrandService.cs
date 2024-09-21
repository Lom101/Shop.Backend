using Shop.WebAPI.Dtos.Brand;
using Shop.WebAPI.Dtos.Brand.Request;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Services;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _brandRepository;

    public BrandService(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
    {
        var brands = await _brandRepository.GetAllAsync();
        return brands.Select(b => new BrandDto
        {
            Id = b.Id,
            Name = b.Name
        });
    }

    public async Task<BrandDto> CreateBrandAsync(CreateBrandRequest request)
    {
        var brand = new Brand
        {
            Name = request.Name
        };
        
        await _brandRepository.AddAsync(brand);
        return new BrandDto { Id = brand.Id, Name = brand.Name };
    }

    public async Task<BrandDto> UpdateBrandAsync(UpdateBrandRequest request)
    {
        var brand = await _brandRepository.GetByIdAsync(request.Id);
        if (brand == null)
            return null;

        brand.Name = request.Name;

        await _brandRepository.UpdateAsync(brand);
        return new BrandDto { Id = brand.Id, Name = brand.Name };
    }

    public async Task<bool> DeleteBrandAsync(int id)
    {
        var brand = await _brandRepository.GetByIdAsync(id);
        if (brand == null)
            return false;

        await _brandRepository.DeleteAsync(brand);
        return true;
    }
}

