using Shop.WebAPI.Dtos.OrderItem;

namespace Shop.WebAPI.Dtos;

public class CreatePaymentIntentRequest
{
    public string UserId { get; set; }
    public List<CreateOrderItemRequest> OrderItems { get; set; }
}