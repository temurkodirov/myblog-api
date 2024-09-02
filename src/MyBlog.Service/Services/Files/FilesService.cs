using Microsoft.AspNetCore.Http;
using MyBlog.Service.Common.Helpers;
using MyBlog.Service.Interfaces.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Hosting;
namespace MyBlog.Service.Services.Files;

public class FilesService : IFilesService
{
    private readonly string ROOTPATH;

    public FilesService(IHostEnvironment env)
    {
        ROOTPATH = env.ContentRootPath;
    }
    public async Task<bool> DeleteImageAsync(string subpath)
    {
        string path = Path.Combine(ROOTPATH, subpath);

        if (File.Exists(path))
        {
            File.Delete(path);
            await Task.CompletedTask;

            return true;
        }

        return false;
    }

    public async Task<string> UploadImageAsync(IFormFile image, string rootpath)
    {
        string newImageName = MediaHelper.MakeImageName(image.FileName);
        string subPath = Path.Combine("wwwroot", "Images", rootpath, newImageName);
        string path = Path.Combine(ROOTPATH, subPath);

        // Ensure that the directory exists before creating the file
        string directoryPath = Path.GetDirectoryName(path);
        if (!Directory.Exists(directoryPath))
        {
            // If it doesn't exist, create it
            Directory.CreateDirectory(directoryPath);
        }

        var stream = new FileStream(path, FileMode.Create);
        await image.CopyToAsync(stream);
        stream.Close();

        return subPath;
    }
}
