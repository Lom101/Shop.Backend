using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Repository.Interfaces;

public interface IPhotoRepository
{
    Task<Photo> UploadPhoto(Model model, IFormFile file);
    Task<Photo?> GetPhotoByIdAsync(int id);
    Task<Photo> UpdatePhoto(Photo photo, IFormFile file);
    bool DeletePhoto(string fileName);
}