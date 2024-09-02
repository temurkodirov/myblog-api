namespace MyBlog.Repository.Entities;

public class CategoryEntity : Auditable
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long ParentId { get; set; }
}
