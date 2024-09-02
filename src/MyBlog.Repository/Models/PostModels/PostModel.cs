using MyBlog.Repository.Models.PostImageModels;

namespace MyBlog.Repository.Models.PostModels;

public class PostModel
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public long UserId { get; set; }
    public long CategoryId { get; set; }
    public List<PostImageModel>? Images { get; set; } = new List<PostImageModel>();
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }

}
