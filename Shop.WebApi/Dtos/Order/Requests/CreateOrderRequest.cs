using Shop.WebAPI.Dtos.OrderItem;
using Shop.WebAPI.Dtos.OrderItem.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Enums;

namespace Shop.WebAPI.Dtos.Order.Requests;

public class CreateOrderRequest
{
    public int UserId { get; set; }
    
    public OrderStatus Status { get; set; }
    
    public ICollection<CreateOrderItemRequest> Items { get; set; }
    
}

//public ShippingMethods ShippingMethod { get; set; }


// public DateTime Created { get; set; }
// public OrderStatus Status { get; set; }
// public decimal TotalAmount { get; set; }
//
// public string UserId { get; set; }
//
// public IEnumerable<GetOrderItemResponse> OrderItems { get; set; }