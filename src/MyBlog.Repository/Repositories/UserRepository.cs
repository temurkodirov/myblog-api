using MyBlog.Repository.Contexts;
using MyBlog.Repository.Entities;
using MyBlog.Repository.Interfaces;

namespace MyBlog.Repository.Repositories;

public class UserRepository : GenericRepository<UserEntity>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
        
    }
}
