namespace MyBlog.Repository.Models.CommentModels;

public class CommentUpdateModel
{
    public long Id { get; set; }
    public long PostId { get; set; }
    public string Content { get; set; } = string.Empty;
}
