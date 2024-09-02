using MyBlog.Repository.Entities;

namespace MyBlog.Domain.Entities;

public class PostEntity : Auditable
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public long UserId { get; set; }
    public UserEntity User { get; set; }
    public long CategoryId { get; set; }
    public CategoryEntity Category { get; set; }

}
