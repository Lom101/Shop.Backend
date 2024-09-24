using Shop.WebAPI.Dtos.Product;

namespace Shop.WebAPI.Dtos;

public class ModelDto
{
    public int Id { get; set; }
    public double Price { get; set; }

    public int ProductId { get; set; }
    public ProductDto Product { get; set; }
    
    public int ColorId { get; set; }
    public ColorDto Color { get; set; }
    
    
    public ICollection<ModelSizeDto> ModelSizes { get; set; }
    public ICollection<PhotoDto> Photos { get; set; }
}