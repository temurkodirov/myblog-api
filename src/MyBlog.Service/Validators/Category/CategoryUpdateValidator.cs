using FluentValidation;
using MyBlog.Repository.Models.CategoryModels;

namespace MyBlog.Service.Validators.Category;

public class CategoryUpdateValidator : AbstractValidator<CategoryUpdateModel>
{
    public CategoryUpdateValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().NotNull().WithMessage("Name field is required")
            .MinimumLength(3).WithMessage("Name must be more than 3 characters")
            .MaximumLength(50).WithMessage("Name must be less than 50 characters");

        RuleFor(dto => dto.Description).NotEmpty().NotNull().WithMessage("Description field is required")
            .MinimumLength(3).WithMessage("Description should be more than 3 characters");
    }
}