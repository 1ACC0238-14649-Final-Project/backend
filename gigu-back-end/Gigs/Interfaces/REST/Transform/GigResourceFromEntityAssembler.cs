using Gigs.Domain.Models.Entities;
using Gigs.Interfaces.REST.Resources;
using System.Collections.Generic;
using System.Linq;

namespace Gigs.Interfaces.REST.Transform
{
    public static class GigResourceFromEntityAssembler
    {
        public static GigResource? ToResourceFromEntity(Gig? entity)
        {
            if (entity == null) return null;

            return new GigResource
            {
                Id = entity.Id,
                Image = entity.Image,
                Title = entity.Title,
                Description = entity.Description,
                Price = entity.Price,
                CreatedAt = entity.CreatedAt,
                SellerId = entity.SellerId,
                Category = entity.Category,
                DeliveryDays = entity.DeliveryDays,
                Tags = entity.Tags?.ToList() ?? new List<string>(),
                IsResponsive = entity.IsResponsive,
                RevisionCount = entity.RevisionCount,
                PageCount = entity.PageCount,
                ExtraFeatures = entity.ExtraFeatures?.ToList() ?? new List<string>(),
                CustomAnimations = entity.CustomAnimations
            };
        }

        public static IEnumerable<GigResource> ToResourceFromEntities(IEnumerable<Gig>? entities)
        {
            return entities?
                       .Select(ToResourceFromEntity)
                       .Where(resource => resource != null)!
                   ?? Enumerable.Empty<GigResource>();
        }

        public static PagedResource<GigResource> ToPagedResourceFromEntities(
            IEnumerable<Gig>? entities,
            int totalItems,
            int currentPage,
            int pageSize)
        {
            return new PagedResource<GigResource>
            {
                Items = ToResourceFromEntities(entities),
                TotalItems = totalItems,
                CurrentPage = currentPage,
                PageSize = pageSize
            };
        }
    }
}