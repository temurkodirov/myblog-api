namespace MyBlog.Repository.Models.CommentModels;

public class CommentCreateModel
{
    public long PostId { get; set; }
    public string Content { get; set; } = string.Empty;
}
