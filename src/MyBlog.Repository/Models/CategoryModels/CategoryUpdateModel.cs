namespace MyBlog.Repository.Models.CategoryModels;

public class CategoryUpdateModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long ParentId { get; set; }
}
