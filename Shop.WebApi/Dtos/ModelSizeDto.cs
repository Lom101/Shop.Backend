namespace Shop.WebAPI.Dtos;

public class ModelSizeDto
{
    public int ModelId { get; set; }
    public ModelDto Model { get; set; }

    public int SizeId { get; set; }
    public SizeDto Size { get; set; }
}