using MyBlog.Repository.Models.CategoryModels;
using MyBlog.Repository.Utilities.Listing;

namespace MyBlog.Service.Interfaces.Categories;

public interface ICategoryService
{
    public Task<bool> CreateAsync(CategoryCreateModel projectCategory);
    public Task<PagedList<CategoryModel>> GetAllAsync(CategoryFilterParams filterParams);
    public Task<bool> UpdateAsync(long id, CategoryUpdateModel category);
    public Task<bool> DeleteAsync(long id);
    public Task<CategoryModel> GetByIdAsync(long id);
}
