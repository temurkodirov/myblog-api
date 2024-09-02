using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Repository.Models.UserModels;
using MyBlog.Service.Dtos.User;
using MyBlog.Service.Interfaces.Users;

namespace MyBlog.Api.Controllers;

[Route("api/people")]
[ApiController]
public class PeopleController : ControllerBase
{
    private readonly IUserService _userService;

    public PeopleController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("get-all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllAsync([FromQuery] UserFilterParams filterParams)
    {
        var result = await _userService.GetAllAsync(filterParams);
        return Ok(result.ToPagedListData());
    }

    [HttpGet("get/{id}")]
    [AllowAnonymous]

    public async Task<IActionResult> GetAsync(long id)
    {
        var result = await _userService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync([FromForm] UserUpdateDto userUpdateDto)
    {
        var result = await _userService.UpdateAsync(userUpdateDto);
        return Ok(result);
    }
}
