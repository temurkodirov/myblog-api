using MyBlog.Repository.Utilities.Listing;

namespace MyBlog.Repository.Models.CategoryModels;

public class CategoryFilterParams : QueryStringParameters
{
    public long? ParentId { get; set; }
}
