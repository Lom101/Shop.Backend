namespace Shop.WebAPI.Dtos.Address.Requests;

public class CreateAddressRequest
{
    public string AddressName { get; set; }
    // public string City { get; set; }
    // public string State { get; set; }
    // public string ZipCode { get; set; }
    public string UserId { get; set; }
}