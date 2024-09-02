using MyBlog.Repository.Contexts;
using MyBlog.Repository.Entities;
using MyBlog.Repository.Interfaces;

namespace MyBlog.Repository.Repositories;

public class CategoryRepository : GenericRepository<CategoryEntity>, ICategoryRepository
{
    public CategoryRepository(AppDbContext dbContext) : base(dbContext)
    {
        
    }
}
