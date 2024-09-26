using Shop.WebAPI.Dtos.Color.Responses;
using Shop.WebAPI.Dtos.Photo.Responses;
using Shop.WebAPI.Dtos.Product.Responses;
using Shop.WebAPI.Dtos.Size.Responses;

namespace Shop.WebAPI.Dtos.Model.Response;

public class GetModelResponse
{
    public int Id { get; set; }
    public double Price { get; set; }

    public int ProductId { get; set; }
    //public GetProductResponse Product { get; set; } //изза этого была ошибка циклическая
    
    public int ColorId { get; set; }    
    public GetColorResponse Color { get; set; }
    
    
    //public ICollection<GetSizeResponse> Sizes { get; set; }
    public ICollection<GetPhotoResponse> Photos { get; set; }
}