using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.WebAPI.Repository;

public class PhotoRepository : IPhotoRepository
{
    private readonly ShopApplicationContext _context;
    private readonly IWebHostEnvironment _environment;

    public PhotoRepository(ShopApplicationContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<Photo> UploadPhoto(Model model, IFormFile file)
    {
        var fileName = Path.GetFileName(file.FileName);
        var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var photo = new Photo
        {
            FileName = fileName,
            FilePath = filePath,
            Length = file.Length,
            ModelId = model.Id,
            Model = model
        };

        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();

        return photo;
    }

    public async Task<Photo?> GetPhotoByIdAsync(int id)
    {
        return await _context.Photos.FindAsync(id);
    }

    public async Task<Photo> UpdatePhoto(Photo photo, IFormFile file)
    {
        // Удаление старого файла
        if (File.Exists(photo.FilePath))
        {
            File.Delete(photo.FilePath);
        }

        var fileName = Path.GetFileName(file.FileName);
        var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        photo.FileName = fileName;
        photo.FilePath = filePath;
        photo.Length = file.Length;

        _context.Photos.Update(photo);
        await _context.SaveChangesAsync();

        return photo;
    }

    public bool DeletePhoto(string fileName)
    {
        var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            return true;
        }
        return false;
    }
}