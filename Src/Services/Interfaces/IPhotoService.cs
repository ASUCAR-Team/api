using CloudinaryDotNet.Actions;

namespace api.Services.Interfaces;

public interface IPhotoService
{
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
}