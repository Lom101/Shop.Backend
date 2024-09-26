namespace Shop.WebAPI.Dtos.Photo.Requests;

public class UpdatePhotoRequest
{
    public int Id { get; set; }
    public int ModelId { get; set; }
    public IFormFile File { get; set; }
}