using Shop.WebAPI.Dtos.Product;
using Shop.WebAPI.Dtos.Product.Responses;
using Shop.WebAPI.Dtos.User.Responses;

namespace Shop.WebAPI.Dtos.Comment.Responses;

public class GetCommentResponse
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime Created { get; set; }

    public int ProductId { get; set; }
    public GetProductResponse Product { get; set; }
    public string UserId { get; set; }
    public GetApplicationUserResponse User { get; set; }
}