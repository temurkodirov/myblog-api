using MyBlog.Domain.Entities;
using MyBlog.Repository.Contexts;
using MyBlog.Repository.Interfaces;

namespace MyBlog.Repository.Repositories;

public class PostRepository : GenericRepository<PostEntity>, IPostRepository
{
    public PostRepository(AppDbContext dbContext) : base(dbContext)
    {
        
    }
}
