using MyBlog.Repository.Models.CommentModels;
using MyBlog.Repository.Utilities.Listing;

namespace MyBlog.Service.Interfaces.Comments;

public interface ICommentService
{
    public Task<bool> CreateAsync(CommentCreateModel comment);
    public Task<PagedList<CommentModel>> GetAllAsync(CommentFilterModel filterParams);
    public Task<bool> UpdateAsync(long id, CommentUpdateModel comment);
    public Task<bool> DeleteAsync(long id);
    public Task<CommentModel> GetByIdAsync(long id);
}
