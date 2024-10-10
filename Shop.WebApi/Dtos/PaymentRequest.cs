using Shop.WebAPI.Dtos.OrderItem;

namespace Shop.WebAPI.Dtos;

public class PaymentRequest
{
    public string UserId { get; set; }
    public List<CreateOrderItemRequest> OrderItems { get; set; }
}