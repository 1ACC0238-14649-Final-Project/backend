using FluentValidation;
using Gigs.Domain.Models.Commands;

namespace Gigs.Domain.Models.Validators
{
    public class UpdateGigCommandValidator : AbstractValidator<UpdateGigCommand>
    {
        public UpdateGigCommandValidator()
        {
            // Validación básica del ID
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Valid Gig ID is required");

            // Validación para Image (base64)
            //RuleFor(x => x.Image)
                //.NotEmpty().WithMessage("Image is required")
                //.Must(BeValidBase64).WithMessage("Image must be a valid base64 string");

            // Validación para Title
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            // Validación para Description
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");

            // Validación para Price
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            // Validación para Category
            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required")
                .MaximumLength(100).WithMessage("Category cannot exceed 100 characters");

            // Validación mejorada para DeliveryDays
            RuleFor(x => x.DeliveryDays)
                .InclusiveBetween(1, 365).WithMessage("Delivery days must be between 1 and 365");

            // Validación para Tags
            RuleFor(x => x.Tags)
                .Must(tags => tags == null || tags.Count <= 10)
                .WithMessage("Cannot have more than 10 tags")
                .ForEach(tag => tag.NotEmpty().WithMessage("Tag cannot be empty"));

            // Validación para IsResponsive (opcional)
            RuleFor(x => x.IsResponsive)
                .NotNull().WithMessage("Responsive flag must be specified");

            // Validación para RevisionCount
            RuleFor(x => x.RevisionCount)
                .GreaterThanOrEqualTo(0).WithMessage("Revision count cannot be negative");

            // Validación para PageCount
            RuleFor(x => x.PageCount)
                .GreaterThan(0).WithMessage("Page count must be at least 1");

            // Validación para ExtraFeatures
            RuleFor(x => x.ExtraFeatures)
                .Must(features => features == null || features.Count <= 5)
                .WithMessage("Cannot have more than 5 extra features")
                .ForEach(feature => feature.NotEmpty().WithMessage("Feature cannot be empty"));

            // Validación para CustomAnimations (opcional)
            RuleFor(x => x.CustomAnimations)
                .NotNull().WithMessage("Custom animations flag must be specified");
        }

        private bool BeValidBase64(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64)) return false;
            
            try
            {
                var base64Data = base64.Split(',')[^1]; // Extrae solo la parte base64
                Convert.FromBase64String(base64Data);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}