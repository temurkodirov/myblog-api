using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Repository.Models.CommentModels;
using MyBlog.Service.Interfaces.Comments;
using MyBlog.Service.Validators.Coment;

namespace MyBlog.Api.Controllers;

[Route("api/coment")]
[ApiController]
public class ComentController : ControllerBase
{
    private readonly ICommentService _comentService;

    public ComentController(ICommentService commentService)
    {
        _comentService = commentService;
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateComentAsync([FromBody] CommentCreateModel model)
    {
        var comentValidator = new ComentValidator();
        var result = comentValidator.Validate(model);

        if (result.IsValid) return Ok(await _comentService.CreateAsync(model));
        else return BadRequest(result.Errors);
    }
    [HttpGet("get")]
    public async Task<IActionResult> GetByIdAsync(long id)
    {
        var result = await _comentService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync([FromQuery] CommentFilterModel filterParams)
    {
        try
        {
            var result = await _comentService.GetAllAsync(filterParams);
            return Ok(result.ToPagedListData());
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpDelete("delete")]
    [Authorize]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        var result = await _comentService.DeleteAsync(id);
        return Ok(result);
    }

}
