namespace Shop.WebAPI.Dtos.Photo.Requests;

public class CreatePhotoRequest
{
    public int ModelId { get; set; }
    public IFormFile File { get; set; }
}