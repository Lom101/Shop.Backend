using Shop.WebAPI.Dtos.OrderItem.Responses;
using Shop.WebAPI.Dtos.User.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Enums;

namespace Shop.WebAPI.Dtos.Order.Responses;

public class GetOrderResponse
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string UserId { get; set; }
    public GetApplicationUserResponse User { get; set; }
    
    public int AddressId { get; set; } // ID адреса доставки
    public string ContactPhone { get; set; } // Контактный телефон
    public IEnumerable<GetOrderItemResponse> OrderItems { get; set; }
}