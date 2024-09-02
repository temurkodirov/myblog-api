using FluentValidation;
using MyBlog.Repository.Models.UserModels;

namespace MyBlog.Service.Validators;

public class UserLoginValidator : AbstractValidator<UserLoginModel>
{
    public UserLoginValidator()
    {
        RuleFor(dto => dto.Email).EmailAddress().WithMessage("Write correct email address");

        RuleFor(dto => dto.Password).Must(password => PasswordValidator.IsStrongPassword(password).IsValid)
             .WithMessage("Password is not strong password!");
    }
}
