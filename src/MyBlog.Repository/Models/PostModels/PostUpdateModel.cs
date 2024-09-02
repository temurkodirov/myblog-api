using Microsoft.AspNetCore.Http;

namespace MyBlog.Repository.Models.PostModels;

public class PostUpdateModel
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public long CategoryId { get; set; }
    public List<IFormFile>? Images { get; set; }
}
