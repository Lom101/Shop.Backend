using System.ComponentModel.DataAnnotations;

namespace Shop.WebAPI.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime Created { get; set; }
    
    [Range(1, 5, ErrorMessage = "Рейтинг должен быть от 1 до 5.")]
    public int Rating { get; set; }  // Значение рейтинга от 1 до 5
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}