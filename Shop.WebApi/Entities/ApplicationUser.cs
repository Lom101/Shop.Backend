using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace Shop.WebAPI.Entities;

public class ApplicationUser : IdentityUser
{
    
    public DateTime Created { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ApplicationUser()
    {
        Created = DateTime.Now;
        Orders = new Collection<Order>();
        Comments = new Collection<Comment>();
    }
}

// public int Id { get; set; }
// public string UserName { get; set; }
// public string Email { get; set; }
// public string? ShippingAddress { get; set; }
// public string? BillingAddress { get; set; }
//
