using MyBlog.Repository.Utilities.Listing;

namespace MyBlog.Repository.Models.PostImageModels;

public class PostImageFilterModel : QueryStringParameters
{
  public long? PostId { get; set; }
}
