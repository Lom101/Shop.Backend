using System.Collections.ObjectModel;

namespace Shop.WebAPI.Entities;

public class Model
{
    public int Id { get; set; }
    public double Price { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int ColorId { get; set; }
    public Color Color { get; set; }
    
    
    public ICollection<ModelSize> ModelSizes { get; set; }
    public ICollection<Photo> Photos { get; set; }
    
    public Model()
    {
        ModelSizes = new Collection<ModelSize>();
        Photos = new Collection<Photo>();
    }
}