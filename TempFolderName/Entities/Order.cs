using Shop.WebAPI.Enums;

namespace Shop.WebAPI.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public OrderStatus Status { get; set; }
    public decimal? TotalAmount => (decimal)OrderItems.Sum(item => item.Amount * item.Quantity);
    
    
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    
    public string PaymentIntentId { get; set; } // ID платежного намерения Stripe
    public int AddressId { get; set; }
    public Address ShippingAddress { get; set; } 
    public string ContactPhone { get; set; } 
    
    public ICollection<OrderItem> OrderItems { get; set; }

    public Order()
    {
        Created = DateTime.UtcNow;
    }
}