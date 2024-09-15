using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Dtos.Address;

public class AddressDto
{
    public int Id { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string UserId { get; set; }
    public UserDto User { get; set; }
}