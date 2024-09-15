using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Dtos.Order.Responses;

public class GetOrderResponse
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public UserDto User { get; set; }   // Для отображения имени пользователя
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
    public IEnumerable<OrderItemDto> OrderItems { get; set; }
}