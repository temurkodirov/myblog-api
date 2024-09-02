using MyBlog.Repository.Entities;

namespace MyBlog.Domain.Entities;

public class CommentEntity : Auditable
{
    public long AuthorId { get; set; }
    public long PostId { get; set; }
    public string Content { get; set; } = string.Empty;
}
