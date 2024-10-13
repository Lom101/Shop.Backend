using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace Shop.WebAPI.Entities;

public class ApplicationUser : IdentityUser
{
    public DateTime Created { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Review> Comments { get; set; }
    public ApplicationUser()
    {
        Created = DateTime.Now;
        Orders = new Collection<Order>();
        Comments = new Collection<Review>();
    }
}
