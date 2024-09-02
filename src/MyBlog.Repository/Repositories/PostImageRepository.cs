using MyBlog.Domain.Entities;
using MyBlog.Repository.Contexts;
using MyBlog.Repository.Interfaces;

namespace MyBlog.Repository.Repositories;

public class PostImageRepository : GenericRepository<PostImageEntity>, IPostImageRepository
{
    public PostImageRepository(AppDbContext dbContext) : base(dbContext)
    {
        
    }
}
