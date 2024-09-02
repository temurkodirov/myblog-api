namespace MyBlog.Repository.Models.PostImageModels;

public class PostImageModel
{
    public long Id { get; set; }
    public long PostId { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
