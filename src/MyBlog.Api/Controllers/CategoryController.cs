using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Repository.Models.CategoryModels;
using MyBlog.Service.Interfaces.Categories;
using MyBlog.Service.Validators.Category;

namespace MyBlog.Api.Controllers;

[Route("api/category")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }


    [HttpPost("create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryCreateModel category)
    {
        var categoryCreateValidator = new CategoryCreateValidator();
        var result = categoryCreateValidator.Validate(category);
       
        if (result.IsValid) return Ok(await _categoryService.CreateAsync(category));
        else return BadRequest(result.Errors);
    }


    [HttpPost("update/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCategoryAsync(long id , [FromBody] CategoryUpdateModel category)
    {
        var categoryUpdateValidator = new CategoryUpdateValidator();
        var result = categoryUpdateValidator.Validate(category);
        
        if (result.IsValid) return Ok(await _categoryService.UpdateAsync(id, category));
        else return BadRequest(result.Errors);
    }


    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCategoryAsync(long id)
    {
        var result = await _categoryService.DeleteAsync(id);

        return Ok(result);
    }


    [HttpGet("get-all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] CategoryFilterParams filterParams)
    {
        try
        {
            var result = await _categoryService.GetAllAsync(filterParams);
            return Ok(result.ToPagedListData());
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    [HttpGet("get/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAsync(long id)
    {
        var result = await _categoryService.GetByIdAsync(id);
        
        return Ok(result);
    }
}
