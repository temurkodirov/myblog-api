using Microsoft.Extensions.Caching.Memory;
using MyBlog.Domain.Exceptions;
using MyBlog.Domain.Exceptions.Auth;
using MyBlog.Repository.Entities;
using MyBlog.Repository.Interfaces;
using MyBlog.Repository.Models.UserModels;
using MyBlog.Service.Common.Helpers;
using MyBlog.Service.Common.Security;
using MyBlog.Service.Dtos.Notifications;
using MyBlog.Service.Dtos.User;
using MyBlog.Service.Interfaces;
using MyBlog.Service.Interfaces.Notifications;
using MyBlog.Service.Interfaces.User;
namespace MyBlog.Service.Services.Users;

public class AuthUserService : IAuthUserService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMailSender _mailSender;
    IUserTokenService _userTokenService;
    private const int CACHED_MINUTES_FOR_REGISTER = 60;
    private const int CACHED_MINUTES_FOR_VERIFICATION = 5;
    private const string REGISTER_CACHE_KEY = "register_";
    private const string VERIFY_REGISTER_CACHE_KEY = "verify_register_";
    private const int VERIFICATION_MAXIMUM_ATTEMPTS = 3;
    public AuthUserService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, 
        IMailSender mailSender, IUserTokenService userTokenService)
    {
        _unitOfWork = unitOfWork;
        _memoryCache = memoryCache;
        _mailSender = mailSender;
        _userTokenService = userTokenService;
    }

#pragma warning disable
    public async Task<(bool Result, int CashedMinutes)> RegisterAsync(UserCreateModel registerDto)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(x => x.Email.ToLower().Equals(registerDto.Email));
        if (user is not null) throw new AlreadyExistException("Email already registered ");

        if (_memoryCache.TryGetValue(REGISTER_CACHE_KEY + registerDto.Email, out UserCreateModel cachedUser))
        {
            registerDto.FirstName = cachedUser.FirstName;
            _memoryCache.Remove(REGISTER_CACHE_KEY + registerDto.Email);
        }
        else 
        {
            _memoryCache.Set(REGISTER_CACHE_KEY + registerDto.Email, registerDto, TimeSpan.FromMinutes(CACHED_MINUTES_FOR_REGISTER));
        }

        return (Result: true, CashedMinutes: CACHED_MINUTES_FOR_REGISTER);
    }
  


    public async Task<(bool Result, int CashedVerificationMinutes)> SendCodeForRegisterAsync(string mail)
    {
        if (_memoryCache.TryGetValue(REGISTER_CACHE_KEY + mail, out UserCreateModel registerDto))
        {
            UserVerificationDto verificationDto = new UserVerificationDto();
            verificationDto.Attempt = 0;
            verificationDto.CreatedAt = TimeHelper.GetDateTime();

            verificationDto.Code = CodeGenerator.GenerateRandomNumber();
            verificationDto.Code = 12345;

            if (_memoryCache.TryGetValue(VERIFY_REGISTER_CACHE_KEY + mail, out UserVerificationDto oldDto))
            {
                _memoryCache.Remove(VERIFY_REGISTER_CACHE_KEY + mail);
            }
            _memoryCache.Set(VERIFY_REGISTER_CACHE_KEY + mail, verificationDto,
                TimeSpan.FromMinutes(CACHED_MINUTES_FOR_VERIFICATION));

            EmailMessage emailSms = new EmailMessage();
            emailSms.Title = "Get Ta'lim";
            emailSms.Content = "Verification code : " + verificationDto.Code;
            emailSms.Recipent = mail;

            var mailResult = await _mailSender.SendAsync(emailSms);
            if (mailResult is true) return (Result: true, CachedVerificationMinutes: CACHED_MINUTES_FOR_VERIFICATION);
            else return (Result: false, CachedVerificationMinutes: 0);
        } else throw new ExpiredException(410, "User data expired");
    }

    public async Task<(bool Result, string Token)> VerifyRegisterAsync(string mail, int code)
    {
        if (_memoryCache.TryGetValue(REGISTER_CACHE_KEY + mail, out UserCreateModel registroDto))
        {
            if (_memoryCache.TryGetValue(VERIFY_REGISTER_CACHE_KEY + mail, out UserVerificationDto verificationDto))
            {
                if (verificationDto.Attempt >= VERIFICATION_MAXIMUM_ATTEMPTS)
                    throw new VerificationTooManyRequestsException();
                else if (verificationDto.Code == code)
                {
                    var dbResult = await RegisterToDatabaseAsync(registroDto);
                    if (dbResult is true)
                    {
                        var student = await _unitOfWork.UserRepository.GetAsync(item => item.Email == mail);
                        string token = _userTokenService.GenerateToken(student);
                        return (Result: true, Token: token);
                    }
                    else return (Result: false, Token: "");
                }
                else
                {
                    _memoryCache.Remove(VERIFY_REGISTER_CACHE_KEY + mail);
                    verificationDto.Attempt++;
                    _memoryCache.Set(VERIFY_REGISTER_CACHE_KEY + mail, verificationDto,
                        TimeSpan.FromMinutes(CACHED_MINUTES_FOR_VERIFICATION));
                    return (Result: false, Token: "");
                }
            }
            else throw new ExpiredException(410, "Verification code expired");
        }
        else throw new ExpiredException(410, "User data expired");
    }

    private async Task<bool> RegisterToDatabaseAsync(UserCreateModel registroDto)
    {
        var user = new UserEntity();
        user.FirstName = registroDto.FirstName;
        user.LastName = registroDto.LastName;
        user.Email = registroDto.Email;
        user.IsEmailConfirmed = true;

        var hasherResult = PasswordHasher.Hash(registroDto.Password);
        user.PasswordHash = hasherResult.Hash;
        user.Salt = hasherResult.Salt;

        user.CreatedAt = user.UpdatedAt = TimeHelper.GetDateTime();

        try
        {
            // Add the user to the repository
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.CommitAsync();

            // Check if the result indicates success
            if (user.Id > 0)
            {
                // Commit the transaction if using Unit of Work pattern
                return true;
            }
            else
            {
                // Log the failure or handle it accordingly
                return false;
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions, such as logging the error
            Console.WriteLine($"Error registering user: {ex.Message}");
            return false;
        }
    }

    public async Task<(bool Result, string Token)> LoginAsync(UserLoginModel loginDto)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(item => item.Email == loginDto.Email);
        if (user is null) throw new CustomException(400, "Email or password incorrect");

        var hasherResult = PasswordHasher.Verify(loginDto.Password, user.PasswordHash, user.Salt);
        if (hasherResult == false) throw new CustomException(400, "Email or password incorrect");

        string token = _userTokenService.GenerateToken(user);

        return (Result: true, Token: token);
    }
}
