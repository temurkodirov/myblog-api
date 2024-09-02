using MyBlog.Repository.Utilities.Listing;

namespace MyBlog.Repository.Models.PostModels;

public class PostFilterParams : QueryStringParameters
{
    public string SearchText { get; set; } = string.Empty;
    public long? CategoryId { get; set; }
    public long? UserId { get; set; }
}
