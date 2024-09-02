using MyBlog.Domain.Entities;
using MyBlog.Repository.Contexts;
using MyBlog.Repository.Interfaces;

namespace MyBlog.Repository.Repositories;

public class CommentRepository : GenericRepository<CommentEntity>, ICommentRepository
{
    public CommentRepository(AppDbContext dbContext) : base(dbContext)
    {
        
    }
}
