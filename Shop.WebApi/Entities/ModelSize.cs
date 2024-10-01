namespace Shop.WebAPI.Entities;

// таблица связи многие ко многим
public class ModelSize
{
    public int Id { get; set; }
    
    public int ModelId { get; set; }
    public Model Model { get; set; }

    public int SizeId { get; set; }
    public Size Size { get; set; }
    
    public int StockQuantity { get; set; } // Количество на складе для конкретного размера модели
}