using Shop.WebAPI.Enums;

namespace Shop.WebAPI.Entities;

// таблица в которой находятся все элементы всех заказов
public class OrderItem
{
    public int Id { get; set; }
    
    public int Quantity { get; set; }
    public double Amount { get; set; }
    
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }

    public int ModelId { get; set; }
    public virtual Model Model { get; set; }

    public int SizeId { get; set; }
    public virtual Size Size { get; set; }
}