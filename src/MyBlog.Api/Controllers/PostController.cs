using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Repository.Models.PostModels;
using MyBlog.Service.Interfaces.Posts;

namespace MyBlog.Api.Controllers;

[Route("api/post")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreatePostAsync([FromForm] PostCreateModel postCreateModel)
    {
        return Ok(await _postService.CreateAsync(postCreateModel));
    }

    [HttpPost("update/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdatePostAsync(long id,[FromForm] PostUpdateModel postUpdateModel)
    {
        return Ok(await _postService.UpdateAsync(id, postUpdateModel));
    }


    [HttpGet("get-all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllAsync([FromQuery] PostFilterParams filterParams)
    {
        var result = await _postService.GetAllAsync(filterParams);
        return Ok(result.ToPagedListData());
    }

    [HttpGet("get/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAsync(long id)
    {
        var result = await _postService.GetByIdAsync(id);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _postService.DeleteAsync(id);
        
        return Ok(result);
    }

}
