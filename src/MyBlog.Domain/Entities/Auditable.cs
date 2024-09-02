using MyBlog.Domain.Constants;

namespace MyBlog.Repository.Entities;

public class Auditable : BaseEntity
{
    public DateTime CreatedAt { get; set; } = TimeConstants.Now();
    public DateTime UpdatedAt { get; set; } = TimeConstants.Now();
}
