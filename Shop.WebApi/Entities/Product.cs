using System.Collections.ObjectModel;

namespace Shop.WebAPI.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    public int BrandId { get; set; }
    public Brand Brand { get; set; }
    
    public ICollection<Model> Models { get; set; }
    public ICollection<Review> Comments { get; set; }
    
    // В наличие на складе, если есть хоть одна доступная модель на складе
    public bool IsAvailable => Models.Any(m => m.IsAvailable == true); 
    
    public Product()
    {
        Created = DateTime.Now;
        Models = new Collection<Model>();
        Comments = new Collection<Review>();
    }
}