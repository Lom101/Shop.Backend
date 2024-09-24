using Shop.WebAPI.Dtos.Brand;
using Shop.WebAPI.Dtos.Category;

namespace Shop.WebAPI.Dtos.Product.Responses;

public class GetProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    
    public int CategoryId { get; set; }
    public CategoryDto Category { get; set; }
    
    public int BrandId { get; set; }
    public BrandDto Brand { get; set; }
    
    public ICollection<ModelDto> Models { get; set; }
}
