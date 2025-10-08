using Gigs.Domain.Models.Commands;
using Gigs.Interfaces.REST.Resources;

namespace Gigs.Interfaces.REST.Transform
{
    public static class UpdateGigCommandFromResourceAssembler
    {
        public static UpdateGigCommand ToCommandFromResource(UpdateGigResource resource, int gigId)
        {
            return new UpdateGigCommand
            {
                Id = gigId,
                Title = resource.Title,
                Description = resource.Description,
                Price = resource.Price,
                Category = resource.Category,
                DeliveryDays = resource.DeliveryDays
            };
        }
    }
}