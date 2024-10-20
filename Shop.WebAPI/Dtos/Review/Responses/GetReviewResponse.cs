using Shop.WebAPI.Dtos.User.Responses;

namespace Shop.WebAPI.Dtos.Review.Responses;

public class GetReviewResponse
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int Rating { get; set; }  // Значение рейтинга от 1 до 5
    public DateTime Created { get; set; }

    public int ProductId { get; set; }
    //public GetProductResponse Product { get; set; }
    public string UserId { get; set; }
    public GetApplicationUserResponse User { get; set; }
}