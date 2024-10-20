namespace Shop.WebAPI.Dtos.OrderItem;

public class CreateOrderItemRequest
{
    public int ModelId { get; set; }
    public int SizeId { get; set; }
    public int Quantity { get; set; }
}