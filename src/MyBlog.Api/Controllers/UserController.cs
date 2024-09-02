using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Repository.Models.UserModels;
using MyBlog.Service.Interfaces.User;
using MyBlog.Service.Validators;

namespace MyBlog.Api.Controllers;

[Route("api/auth")]
public class UserController : ControllerBase
{
    private readonly IAuthUserService _authService;
    private readonly IIdentityService _identity;
    public UserController(IAuthUserService authUserService, IIdentityService identity)
    {
        _authService = authUserService;
        _identity = identity;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync([FromBody] UserCreateModel registerDto)
    {
        var validator = new UserAuthValidator();
        var result = validator.Validate(registerDto);
        if (result.IsValid)
        {
            var serviceResult = await _authService.RegisterAsync(registerDto);
            return Ok(new { serviceResult.Result, serviceResult.CashedMinutes });
        }
        else return BadRequest(result.Errors);
    }

    [HttpPost("register/send-code")]
    [AllowAnonymous]
    public async Task<IActionResult> SendCodeRegisterAsync(string mail)
    {
        var serviceResult = await _authService.SendCodeForRegisterAsync(mail);
        return Ok(new { serviceResult.Result, serviceResult.CashedVerificationMinutes });
    }


    [HttpPost("register/verify")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyRegisterAsync([FromBody] UserVerifyModel verifyDto)
    {
        var servisResult = await _authService.VerifyRegisterAsync(verifyDto.Email, verifyDto.Code);
        return Ok(new { servisResult.Result, servisResult.Token });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginModel logindto)
    {
        var validator = new UserLoginValidator();
        var valResult = validator.Validate(logindto);
        if (valResult.IsValid == false) return BadRequest(valResult.Errors);

        var serviceResult = await _authService.LoginAsync(logindto);
        return Ok(new { serviceResult.Result, serviceResult.Token });
    }

    [HttpGet("identity")]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(new
        {
            _identity.FirstName,
            _identity.LastName,
            _identity.Email,
            _identity.UserId,
            _identity.ImagePath,
            _identity.IdentityRole
        }
        );
    }

}
