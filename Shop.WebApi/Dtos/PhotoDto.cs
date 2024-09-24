namespace Shop.WebAPI.Dtos;

public class PhotoDto
{
    public int Id { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public long Length { get; set; }
    
    public int ModelId { get; set; }
    public ModelDto Model { get; set; }
}