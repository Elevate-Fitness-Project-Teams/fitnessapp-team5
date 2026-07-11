using UserProfileService.Interfaces.Services;

namespace UserProfileService.Implementation.Services;

public class FileService(IWebHostEnvironment webHostEnvironment) : IFileService
{
    private readonly string _filePath = $"{webHostEnvironment.WebRootPath}/Uploads";

    public async Task<string> UploadAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        var randomFileName = Path.GetRandomFileName();

        var path = Path.Combine(_filePath, randomFileName);

        using var stream = File.Create(path);
        await file.CopyToAsync(stream, cancellationToken);

        return path;
    }

    public Task DeleteAsync(string path)
    {
        if (File.Exists(path))
            File.Delete(path);

        return Task.CompletedTask;
    }
}
