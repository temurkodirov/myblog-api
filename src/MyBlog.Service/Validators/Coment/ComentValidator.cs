using FluentValidation;
using MyBlog.Repository.Models.CommentModels;

namespace MyBlog.Service.Validators.Coment;

public class ComentValidator : AbstractValidator<CommentCreateModel> 
{
    public ComentValidator()
    {
        RuleFor(dto => dto.Content).NotEmpty().NotNull().WithMessage("Name field is required")
           .MinimumLength(1).WithMessage("Name must be more than 3 characters")
           .MaximumLength(9990).WithMessage("Name must be less than 50 characters");

    }
}
