namespace MyBlog.Repository.Models.CommentModels;

public class CommentModel
{
    public long Id { get; set; }
    public long AuthorId { get; set; }
    public long PostId { get; set; }
    public string Content { get; set; } = string.Empty;
    public CommentAuthorModel? AuthorModel { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
