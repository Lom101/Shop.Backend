namespace Shop.WebAPI.Dtos.Model.Request;

public class CreateModelRequest
{
    public double Price { get; set; }
    
    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public ICollection<int> SizeIds { get; set; }
}