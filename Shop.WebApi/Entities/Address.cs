namespace Shop.WebAPI.Entities;

public class Address
{
    public int Id { get; set; }
    public string AddressName { get; set; }
    // public string City { get; set; }
    // public string State { get; set; }
    // public string ZipCode { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}