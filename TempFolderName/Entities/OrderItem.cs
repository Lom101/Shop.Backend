using Shop.WebAPI.Enums;

namespace Shop.WebAPI.Entities;

public class OrderItem
{
    public int Id { get; set; }
    
    public int Quantity { get; set; }
    public double Amount { get; set; }
    
    public int OrderId { get; set; }
    public Order Order { get; set; }

    public int ModelId { get; set; }
    public Model Model { get; set; }

    public int SizeId { get; set; }
    public Size Size { get; set; }
    
    public decimal TotalPrice => (decimal)Amount * Quantity;
}