namespace Shop.WebAPI.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime Created { get; set; }
    
    // тут скорее всего замена на Model
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}