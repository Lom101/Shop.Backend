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
    
    public string PaymentIntentId { get; set; } // ID платежного намерения Stripe
    // Связь с адресом доставки
    public int AddressId { get; set; }
    public Address ShippingAddress { get; set; } // Адрес доставки
    public string ContactPhone { get; set; } // Контактный телефон
    
    public ICollection<OrderItem> OrderItems { get; set; }

    public Order()
    {
        Created = DateTime.UtcNow;
    }
}