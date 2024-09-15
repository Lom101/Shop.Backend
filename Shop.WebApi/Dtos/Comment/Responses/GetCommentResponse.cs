using Shop.WebAPI.Dtos.Product;

namespace Shop.WebAPI.Dtos.Comment.Responses;

public class GetCommentResponse
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ProductId { get; set; }
    public ProductDto Product { get; set; } // Дополнительно, чтобы отобразить название продукта
    public string UserId { get; set; }
    public UserDto User { get; set; } // Дополнительно, чтобы отобразить имя пользователя
}