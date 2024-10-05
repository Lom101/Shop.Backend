using Shop.WebAPI.Dtos.OrderItem;

namespace Shop.WebAPI.Dtos;

public class PaymentRequest
{
    public List<CreateOrderItemRequest> OrderItems { get; set; }
}