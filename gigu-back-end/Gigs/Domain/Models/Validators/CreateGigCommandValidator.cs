using FluentValidation;
using Gigs.Domain.Models.Commands;

namespace Gigs.Domain.Models.Validators
{
    public class CreateGigCommandValidator : AbstractValidator<CreateGigCommand>
    {
        public CreateGigCommandValidator()
        {
            // Validación para Image (base64)
            /*
             RuleFor(x => x.Image)
                .NotEmpty().WithMessage("Image is required")
                .Must(BeValidBase64).WithMessage("Image must be a valid base64 string");
                */

            // Validación existente para Title
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            // Validación existente para Description
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");

            // Cambiado de UserId a SellerId
            /*
             RuleFor(x => x.SellerId)
                .GreaterThan(0).WithMessage("Valid Seller ID is required");
            */

            // Validación existente para Price
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            // Validación existente para Category
            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required")
                .MaximumLength(100).WithMessage("Category cannot exceed 100 characters");

            // Validación existente para DeliveryDays
            RuleFor(x => x.DeliveryDays)
                .InclusiveBetween(1, 365).WithMessage("Delivery days must be between 1 and 365");

            // Validación para Tags
            RuleFor(x => x.Tags)
                .Must(tags => tags == null || tags.Count <= 10)
                .WithMessage("Cannot have more than 10 tags");

            // Validación para RevisionCount
            RuleFor(x => x.RevisionCount)
                .GreaterThanOrEqualTo(0).WithMessage("Revision count cannot be negative");

            // Validación para PageCount
            RuleFor(x => x.PageCount)
                .GreaterThan(0).WithMessage("Page count must be at least 1");

            // Validación para ExtraFeatures
            RuleFor(x => x.ExtraFeatures)
                .Must(features => features == null || features.Count <= 5)
                .WithMessage("Cannot have more than 5 extra features");
        }

        private bool BeValidBase64(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64)) return false;
            
            try
            {
                Convert.FromBase64String(base64.Split(',')[^1]); 
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}