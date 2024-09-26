using Shop.WebAPI.Dtos.Photo.Requests;
using Shop.WebAPI.Dtos.Photo.Responses;
using Shop.WebAPI.Repository.Interfaces;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Services;

public class PhotoService : IPhotoService
{
    private readonly IPhotoRepository _photoRepository;
    private readonly IModelRepository _modelRepository;

    public PhotoService(IPhotoRepository photoRepository, IModelRepository modelRepository)
    {
        _photoRepository = photoRepository;
        _modelRepository = modelRepository;
    }

    public async Task<GetPhotoResponse> UploadPhotoAsync(CreatePhotoRequest request)
    {
        var model = await _modelRepository.GetByIdAsync(request.ModelId);
        if (model == null) return null;

        var photo = await _photoRepository.UploadPhoto(model, request.File);
        return new GetPhotoResponse
        {
            Id = photo.Id,
            FileName = photo.FileName,
            Length = photo.Length,
            Url = "/images/" + photo.FileName // Указываем путь к файлу
        };
    }

    public async Task<GetPhotoResponse?> UpdatePhotoAsync(UpdatePhotoRequest request)
    {
        var model = await _modelRepository.GetByIdAsync(request.ModelId);
        if (model == null) return null;

        var photo = await _photoRepository.GetPhotoByIdAsync(request.Id);
        if (photo == null) return null;

        var updatedPhoto = await _photoRepository.UpdatePhoto(photo, request.File);
        return new GetPhotoResponse
        {
            Id = updatedPhoto.Id,
            FileName = updatedPhoto.FileName,
            Length = updatedPhoto.Length,
            Url = "/images/" + updatedPhoto.FileName
        };
    }

    public async Task<bool> DeletePhotoAsync(int id)
    {
        var photo = await _photoRepository.GetPhotoByIdAsync(id);
        if (photo == null) return false;

        return _photoRepository.DeletePhoto(photo.FileName);
    }
}