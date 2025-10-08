using Gigs.Domain.Models.Commands;
using Gigs.Interfaces.REST.Resources;

namespace Gigs.Interfaces.REST.Transform
{
    public static class CreateGigCommandFromResourceAssembler
    {
        public static CreateGigCommand ToCommandFromResource(CreateGigResource resource)
        {
            return new CreateGigCommand
            {
                Image = resource.Image,
                Title = resource.Title,
                Description = resource.Description,
                Price = resource.Price,
                SellerId = resource.SellerId, 
                Tags = resource.Tags,
                Category = resource.Category,
                DeliveryDays = resource.DeliveryDays,
                IsResponsive = resource.IsResponsive,
                RevisionCount = resource.RevisionCount,
                PageCount = resource.PageCount,
                ExtraFeatures = resource.ExtraFeatures,
                CustomAnimations = resource.CustomAnimations
            };
        }
    }
}