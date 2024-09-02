using Microsoft.AspNetCore.Http;

namespace MyBlog.Service.Interfaces.Files;

public interface IFilesService
{
    public Task<string> UploadImageAsync(IFormFile image, string rootpath);
    public Task<bool> DeleteImageAsync(string subpath);
}
