using Shop.WebAPI.Dtos.Product;

namespace Shop.WebAPI.Dtos.Category.Responses;

public class GetCategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<ProductDto> Products { get; set; }
}
