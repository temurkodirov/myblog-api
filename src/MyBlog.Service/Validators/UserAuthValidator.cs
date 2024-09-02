using FluentValidation;
using MyBlog.Repository.Models.UserModels;

namespace MyBlog.Service.Validators;

public class UserAuthValidator : AbstractValidator<UserCreateModel>
{
    public UserAuthValidator()
    {
        RuleFor(dto => dto.FirstName).NotNull().NotEmpty().WithMessage("Firstname is required!")
           .MaximumLength(30).WithMessage("Firstname must be less than 30 characters");

        RuleFor(dto => dto.LastName).NotNull().NotEmpty().WithMessage("Lastname is required!")
           .MaximumLength(30).WithMessage("Firstname must be less than 30 characters");

        RuleFor(dto => dto.Email).NotNull().NotEmpty().WithMessage("Email required")
            .EmailAddress().WithMessage("Write email address");
        RuleFor(dto => dto.Password).Must(password => PasswordValidator.IsStrongPassword(password).IsValid)
            .WithMessage("Password is not strong!");
    }
}
