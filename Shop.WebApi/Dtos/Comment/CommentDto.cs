using Shop.WebAPI.Dtos.Product;

namespace Shop.WebAPI.Dtos.Comment;

public class CommentDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ProductId { get; set; }
    public ProductDto Product { get; set; }
    public string UserId { get; set; }
    public UserDto User { get; set; }
}