using Shop.WebAPI.Dtos.Product;

namespace Shop.WebAPI.Dtos;

public class OrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public ProductDto Product { get; set; }
    public int OrderId { get; set; }
    public OrderDto Order { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } // Цена за единицу на момент заказа
}