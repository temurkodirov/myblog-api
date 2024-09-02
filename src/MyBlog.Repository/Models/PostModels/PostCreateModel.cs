using Microsoft.AspNetCore.Http;

namespace MyBlog.Repository.Models.PostModels;

public class PostCreateModel
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public long CategoryId { get; set; }
    public List<IFormFile>? Images { get; set; } 
}
