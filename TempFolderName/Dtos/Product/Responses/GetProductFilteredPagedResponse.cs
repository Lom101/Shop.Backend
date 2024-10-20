namespace Shop.WebAPI.Dtos.Product.Responses;

public class GetProductFilteredPagedResponse
{
    public IEnumerable<GetProductResponse> Items { get; set; }
    public int TotalCount { get; set; }
}