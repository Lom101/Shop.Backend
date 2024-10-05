using Shop.WebAPI.Dtos.OrderItem;
using Shop.WebAPI.Dtos.OrderItem.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Enums;

namespace Shop.WebAPI.Dtos.Order.Requests;

public class UpdateOrderRequest
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
    
    // public string UserId { get; set; }
    //
    // public string PaymentIntentId { get; set; } // ID платежного намерения Stripe
    // public int AddressId { get; set; } // ID адреса доставки
    // public string ContactPhone { get; set; } // Контактный телефон
    //
    // public IEnumerable<CreateOrderItemRequest> OrderItems { get; set; }
}