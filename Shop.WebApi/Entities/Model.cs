using System.Collections.ObjectModel;

namespace Shop.WebAPI.Entities;

public class Model
{
    public int Id { get; set; }
    public double Price { get; set; }

    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
    
    public int ColorId { get; set; }
    public virtual Color Color { get; set; }
    
    
    public virtual ICollection<ModelSize> ModelSizes { get; set; }
    public virtual ICollection<Photo> Photos { get; set; }
    
    public Model()
    {
        ModelSizes = new Collection<ModelSize>();
        Photos = new Collection<Photo>();
    }
}