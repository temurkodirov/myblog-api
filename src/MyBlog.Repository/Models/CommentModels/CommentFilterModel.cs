using MyBlog.Repository.Utilities.Listing;

namespace MyBlog.Repository.Models.CommentModels;

public class CommentFilterModel : QueryStringParameters
{
    public long? PostId { get; set; }
}
