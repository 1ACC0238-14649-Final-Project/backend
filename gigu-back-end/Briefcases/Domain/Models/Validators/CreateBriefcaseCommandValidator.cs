using FluentValidation;
using gigu_back_end.Briefcases.Domain.Models.Commands;

namespace gigu_back_end.Briefcases.Domain.Models.Validators;

public class CreateBriefcaseCommandValidator : AbstractValidator<CreateBriefcaseCommand>
{
    public CreateBriefcaseCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(v => v.Description).NotEmpty().Length(10, 100).WithMessage("Description lenght bwtween 10 and 100 characters");
        RuleFor(v => v.PublishDate).LessThan(DateTime.Now).WithMessage("Publish Date must be greater than Publish Date");
    }
}