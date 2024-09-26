using Shop.WebAPI.Dtos.Brand;
using Shop.WebAPI.Dtos.Brand.Response;
using Shop.WebAPI.Dtos.Category;
using Shop.WebAPI.Dtos.Category.Responses;
using Shop.WebAPI.Dtos.Model.Response;

namespace Shop.WebAPI.Dtos.Product.Responses;

public class GetProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    
    public int CategoryId { get; set; }
    public GetCategoryResponse Category { get; set; }
    
    public int BrandId { get; set; }
    public GetBrandResponse Brand { get; set; }
    
    public ICollection<GetModelResponse> Models { get; set; }
}
