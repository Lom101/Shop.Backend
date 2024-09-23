using AutoMapper;
using Shop.WebAPI.Dtos.Brand;
using Shop.WebAPI.Dtos.Brand.Request;
using Shop.WebAPI.Dtos.Brand.Response;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Services;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _brandRepository;
    private readonly IMapper _mapper;

    public BrandService(IBrandRepository brandRepository, IMapper mapper)
    {
        _brandRepository = brandRepository;
        _mapper = mapper;
    }

    // Получение всех брендов
    public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
    {
        var brands = await _brandRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<BrandDto>>(brands); // Маппинг через AutoMapper
    }

    // Получение бренда по ID
    public async Task<GetBrandResponse> GetBrandByIdAsync(int id)
    {
        var brand = await _brandRepository.GetByIdAsync(id);
        if (brand == null)
        {
            return null; // Либо можно бросить исключение
        }
        return _mapper.Map<GetBrandResponse>(brand); // Маппинг через AutoMapper
    }

    // Создание нового бренда
    public async Task<BrandDto> CreateBrandAsync(CreateBrandRequest request)
    {
        var brand = _mapper.Map<Brand>(request); // Маппинг из CreateBrandRequest в Brand
        
        await _brandRepository.AddAsync(brand);
        return _mapper.Map<BrandDto>(brand); // Маппинг из Brand в BrandDto
    }

    // Обновление существующего бренда
    public async Task<BrandDto> UpdateBrandAsync(UpdateBrandRequest request)
    {
        var brand = await _brandRepository.GetByIdAsync(request.Id);
        if (brand == null)
            return null;

        _mapper.Map(request, brand); // Маппинг изменений из UpdateBrandRequest в существующий Brand

        await _brandRepository.UpdateAsync(brand);
        return _mapper.Map<BrandDto>(brand); // Маппинг обновленного Brand в BrandDto
    }

    // Удаление бренда
    public async Task<bool> DeleteBrandAsync(int id)
    {
        var brand = await _brandRepository.GetByIdAsync(id);
        if (brand == null)
            return false;

        await _brandRepository.DeleteAsync(brand);
        return true;
    }
}
