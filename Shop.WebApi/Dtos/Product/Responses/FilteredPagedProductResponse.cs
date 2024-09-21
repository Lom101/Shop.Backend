namespace Shop.WebAPI.Dtos.Product.Responses;

public class FilteredPagedProductResponse
{
    public IEnumerable<ProductDto> Products { get; set; }
    public int TotalItems { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}