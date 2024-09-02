using MyBlog.Domain.Exceptions;
using MyBlog.Repository.Entities;
using MyBlog.Repository.Interfaces;
using MyBlog.Repository.Models.CategoryModels;
using MyBlog.Repository.Utilities.Listing;
using MyBlog.Service.Common.Helpers;
using MyBlog.Service.Interfaces.Categories;

namespace MyBlog.Service.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CreateAsync(CategoryCreateModel category)
    {
        var projectCategoryEntity = new CategoryEntity
        {
            Name = category.Name,
            Description = category.Description,
            ParentId = category.ParentId,
            CreatedAt = TimeHelper.GetDateTime(),
            UpdatedAt = TimeHelper.GetDateTime()
        };

        await _unitOfWork.CategoryRepository.AddAsync(projectCategoryEntity);

        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var category = await _unitOfWork.CategoryRepository.GetAsync(item => item.Id == id);
        if (category is null) throw new NotFoundException("Category not found");
        _unitOfWork.CategoryRepository.Remove(category);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<PagedList<CategoryModel>> GetAllAsync(CategoryFilterParams filterParams)
    {
        var entityItems = await _unitOfWork.CategoryRepository.GetAllByQueryAsync(item =>

      (filterParams.ParentId == null || item.ParentId == filterParams.ParentId),
      null, x => x.CreatedAt,
      filterParams.Order == "desc");
        var items = entityItems.Select(entity => new CategoryModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            ParentId = entity.ParentId,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt

        }).ToList();
        PagedList<CategoryModel> pagedList = PagedList<CategoryModel>.ToPagedListFromQuery(
              filterParams.PageNumber,
              filterParams.PageSize,
              filterParams.Order,
              items.AsQueryable()
          );

        return pagedList;
    }

    public async Task<CategoryModel> GetByIdAsync(long id)
    {
        var category = await _unitOfWork.CategoryRepository.GetAsync(item => item.Id == id);
        if (category is null) throw new NotFoundException("Category not found");

        var categoryDto = new CategoryModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ParentId = category.ParentId,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };

        return categoryDto;
    }

    public async Task<bool> UpdateAsync(long id, CategoryUpdateModel category)
    {
        var dbResult = await _unitOfWork.CategoryRepository.GetAsync(item => item.Id == id);
        if (dbResult is null) throw new NotFoundException("Category not found");

        dbResult.Name = category.Name;
        dbResult.Description = category.Description;
        dbResult.UpdatedAt = TimeHelper.GetDateTime();


        _unitOfWork.CategoryRepository.Update(dbResult);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
