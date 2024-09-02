using MyBlog.Repository.Entities;

namespace MyBlog.Domain.Entities;

public class PostImageEntity : Auditable
{
    public long PostId { get; set; }
    public string PostImagePath { get; set; } = string.Empty;
}
