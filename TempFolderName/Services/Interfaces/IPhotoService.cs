using Shop.WebAPI.Dtos.Photo.Requests;
using Shop.WebAPI.Dtos.Photo.Responses;

namespace Shop.WebAPI.Services.Interfaces;

public interface IPhotoService
{
    Task<GetPhotoResponse> UploadPhotoAsync(CreatePhotoRequest request);
    Task<GetPhotoResponse?> UpdatePhotoAsync(UpdatePhotoRequest request);
    Task<bool> DeletePhotoAsync(int id);
}
