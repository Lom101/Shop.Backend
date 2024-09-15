namespace Shop.WebAPI.Dtos.Comment.Requests;

public class CreateCommentRequest
{
    public string Text { get; set; }
    public int ProductId { get; set; }
    public string UserId { get; set; }
}