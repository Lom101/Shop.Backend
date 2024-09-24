using Shop.WebAPI.Dtos.Brand;
using Shop.WebAPI.Dtos.Category;
using Shop.WebAPI.Dtos.Comment;

namespace Shop.WebAPI.Dtos.Product;

public class ProductDto
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