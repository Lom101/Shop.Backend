namespace Shop.WebAPI.Dtos.Comment.Requests;

public class UpdateCommentRequest
{
    public int Id { get; set; } // Указываем, какой комментарий обновляем
    public string Text { get; set; }
    public int ProductId { get; set; }
    public string UserId { get; set; }
}