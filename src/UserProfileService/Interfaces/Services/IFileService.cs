namespace UserProfileService.Interfaces.Services;

public interface IFileService
{
    Task<string> UploadAsync(IFormFile file, CancellationToken cancellationToken = default);
    Task DeleteAsync(string path);
}
