using Shop.WebAPI.Dtos.Model.Response;
using Shop.WebAPI.Dtos.Order.Responses;
using Shop.WebAPI.Dtos.Size.Responses;

namespace Shop.WebAPI.Dtos.OrderItem.Responses;

public class GetOrderItemResponse
{
    public int Id { get; set; }
    
    public int Quantity { get; set; }
    public double Amount { get; set; }
    
    public int OrderId { get; set; }
    public GetOrderResponse Order { get; set; }

    public int ModelId { get; set; }
    public GetModelResponse Model { get; set; }

    public int SizeId { get; set; }
    public GetSizeResponse Size { get; set; }
}