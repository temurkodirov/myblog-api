using MyBlog.Repository.Entities;

namespace MyBlog.Service.Interfaces.User;

public interface IUserTokenService
{
    public string GenerateToken(UserEntity user);
}
