using MyBlog.Repository.Models.UserModels;

namespace MyBlog.Service.Interfaces.User;

public interface IAuthUserService
{
    public Task<(bool Result, int CashedMinutes)> RegisterAsync(UserCreateModel registerDto);
    public Task<(bool Result, int CashedVerificationMinutes)> SendCodeForRegisterAsync(string mail);
    public Task<(bool Result, string Token)> VerifyRegisterAsync(string mail, int code);
    public Task<(bool Result, string Token)> LoginAsync(UserLoginModel loginDto);
}
