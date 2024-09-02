using Microsoft.AspNetCore.Http;

namespace MyBlog.Repository.Models.PostImageModels;

public class PostImageCreateModel
{
    public IFormFile? Image { get; set; } 
    public long PostId { get; set; }
}
