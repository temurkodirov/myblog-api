using Microsoft.AspNetCore.Http;

namespace MyBlog.Repository.Models.PostImageModels;

public class PostImageUpdateModel
{
    public long Id { get; set; }
    long PostId { get; set; }
    public IFormFile? Image { get; set; }

}
