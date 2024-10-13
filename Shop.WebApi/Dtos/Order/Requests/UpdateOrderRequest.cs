using Shop.WebAPI.Dtos.OrderItem;
using Shop.WebAPI.Dtos.OrderItem.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Enums;

namespace Shop.WebAPI.Dtos.Order.Requests;

public class UpdateOrderRequest
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
}