using MyBlog.Repository.Models.UserModels;
using MyBlog.Repository.Utilities.Listing;
using MyBlog.Service.Dtos.User;

namespace MyBlog.Service.Interfaces.Users;

public interface IUserService
{
    public Task<bool> UpdateAsync(UserUpdateDto userUpdateDto);
    public Task<UserDto> GetByIdAsync(long id);
    public Task<PagedList<UserDto>> GetAllAsync(UserFilterParams filterParams);
}
