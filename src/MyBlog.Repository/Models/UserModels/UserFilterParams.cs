using MyBlog.Repository.Utilities.Listing;

namespace MyBlog.Repository.Models.UserModels;

public class UserFilterParams : QueryStringParameters
{
    public string SearchText { get; set; } = string.Empty;
}
