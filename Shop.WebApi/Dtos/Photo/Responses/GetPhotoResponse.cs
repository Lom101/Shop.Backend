using Shop.WebAPI.Dtos.Model.Response;

namespace Shop.WebAPI.Dtos.Photo.Responses;

public class GetPhotoResponse
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public long Length { get; set; }
    public string Url { get; set; }
    
    // public int ModelId { get; set; }
    // public GetModelResponse Model { get; set; }
}