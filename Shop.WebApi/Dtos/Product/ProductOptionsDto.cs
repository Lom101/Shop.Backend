namespace Shop.WebAPI.Dtos.Product;

public class ProductOptionsDto
{
    public List<string> Categories { get; set; }
    public List<string> Brands { get; set; }
    public List<int> Sizes { get; set; }
    public List<string> Colors { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool InStock { get; set; }

}
