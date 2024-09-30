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
    public ICollection<Comment> Comments { get; set; } // отзывы 
    
    
    public Product()
    {
        Created = DateTime.Now;
        Models = new Collection<Model>();
        Comments = new Collection<Comment>();
    }
}

// public bool IsAvailable { get; set; } // Наличие на складе
// public string Material { get; set; } // Материал
