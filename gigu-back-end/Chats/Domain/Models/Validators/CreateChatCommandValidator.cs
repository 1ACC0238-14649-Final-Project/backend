using FluentValidation;
using Chats.Domain.Models.Commands;

namespace Chats.Domain.Models.Validators;

public class CreateChatCommandValidator : AbstractValidator<CreateChatCommand>
{
    public CreateChatCommandValidator()
    {
        RuleFor(c => c.Content).NotEmpty().WithMessage("Message content is required.");
        RuleFor(c => c.Content).MaximumLength(1000).WithMessage("Content too long.");
    }
}