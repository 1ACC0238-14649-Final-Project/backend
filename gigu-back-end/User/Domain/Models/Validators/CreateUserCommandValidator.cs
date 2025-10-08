using FluentValidation;
using gigu_back_end.User.Application.CommandServices;
using gigu_back_end.Shared.Domain.Models.Commands;

namespace gigu_back_end.User.Domain.Models.Validadors;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(v => v.Lastname).NotEmpty().WithMessage("Lastname is required");
        RuleFor(v => v.Email).EmailAddress().WithMessage("Valid email is required");
        RuleFor(v => v.Password).MinimumLength(6).WithMessage("Password must be at least 6 characters");
        RuleFor(v => v.Role).NotEmpty().WithMessage("Role is required");
    }
}