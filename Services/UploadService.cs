using Microsoft.Extensions.Options;
using public_storage.Configuration;

namespace public_storage.Services;

class UploadService : IUploadService
{
    private readonly string _rootPath;

    public UploadService(IOptions<UploadOptions> options)
    {
        _rootPath = Path.Combine(Directory.GetCurrentDirectory(), options.Value.UploadPath);

        Directory.CreateDirectory(_rootPath);
    }

    public string GetUploadPath() => _rootPath;
}