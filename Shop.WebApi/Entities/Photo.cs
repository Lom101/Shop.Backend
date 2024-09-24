namespace Shop.WebAPI.Entities;

public class Photo
{
    public int Id { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public long Length { get; set; }
    
    public int ModelId { get; set; }
    public virtual Model Model { get; set; }
}