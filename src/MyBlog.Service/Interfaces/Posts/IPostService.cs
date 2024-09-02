using MyBlog.Repository.Models.PostImageModels;
using MyBlog.Repository.Models.PostModels;
using MyBlog.Repository.Utilities.Listing;

namespace MyBlog.Service.Interfaces.Posts;

public interface IPostService
{
    public Task<bool> CreateAsync(PostCreateModel post);
    public Task<bool> UpdateAsync(long id, PostUpdateModel post);
    public Task<bool> DeleteAsync(long id);
    public Task<PagedList<PostModel>> GetAllAsync(PostFilterParams filterParams);
    public Task<PostModel> GetByIdAsync(long id);

}
