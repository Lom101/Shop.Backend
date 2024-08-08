using Microsoft.AspNetCore.Identity;

namespace Shop.WebAPI.Entities;

public class User : IdentityUser
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string ShippingAddress { get; set; }
    public string BillingAddress { get; set; }

    public ICollection<Order> Orders { get; set; }
    public Cart Cart { get; set; }
    public ICollection<Comment> Comments { get; set; }
}
