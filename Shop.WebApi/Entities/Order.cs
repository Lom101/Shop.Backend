using Shop.WebAPI.Enums;

namespace Shop.WebAPI.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    
    
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    
    
    public ICollection<OrderItem> OrderItems { get; set; }
}