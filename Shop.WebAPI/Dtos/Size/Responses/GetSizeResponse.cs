namespace Shop.WebAPI.Dtos.Size.Responses;

public class GetSizeResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    // Поле для количества на складе
    public int StockQuantity { get; set; }

    // Поле для указания доступности размера
    public bool IsAvailable => StockQuantity > 0;
}