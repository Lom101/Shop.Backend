using Shop.WebAPI.Dtos.Product;

namespace Shop.WebAPI.Dtos;

public class OrderItemDto
{
    public int Id { get; set; }
    
    public int Quantity { get; set; }
    public double Amount { get; set; }
    
    public int OrderId { get; set; }
    public OrderDto Order { get; set; }

    public int ModelId { get; set; }
    public ModelDto Model { get; set; }

    public int SizeId { get; set; }
    public SizeDto Size { get; set; }
}