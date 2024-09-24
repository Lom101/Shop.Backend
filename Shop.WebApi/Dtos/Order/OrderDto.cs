using Shop.WebAPI.Entities;
using Shop.WebAPI.Enums;

namespace Shop.WebAPI.Dtos;

public class OrderDto
{
    
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string UserId { get; set; }
    public UserDto User { get; set; }
    
    public IEnumerable<OrderItemDto> OrderItems { get; set; }
}