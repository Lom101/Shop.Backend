namespace Shop.WebAPI.Entities;

public class Address
{
    public int Id { get; set; }
    public string AddressName { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}