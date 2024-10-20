using Shop.WebAPI.Dtos.Brand.Response;
using Shop.WebAPI.Dtos.Category.Responses;
using Shop.WebAPI.Dtos.Color.Responses;
using Shop.WebAPI.Dtos.Size.Responses;

namespace Shop.WebAPI.Dtos.Product.Responses;

public class GetProductFilterOptionsResponse
{
    public IEnumerable<GetCategoryResponse> Categories { get; set; } // Список категорий
    public IEnumerable<GetBrandResponse> Brands { get; set; } // Список брендов
    public IEnumerable<GetSizeResponse> Sizes { get; set; } // Список размеров
    public IEnumerable<GetColorResponse> Colors { get; set; } // Список цветов
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
}
