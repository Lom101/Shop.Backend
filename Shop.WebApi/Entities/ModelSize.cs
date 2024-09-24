namespace Shop.WebAPI.Entities;

// таблица связи многие ко многим
public class ModelSize
{
    public int ModelId { get; set; }
    public virtual Model Model { get; set; }

    public int SizeId { get; set; }
    public virtual Size Size { get; set; }
}