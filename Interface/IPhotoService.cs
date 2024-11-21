using CloudinaryDotNet.Actions;

namespace Registration.Interface
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile imageFile);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
