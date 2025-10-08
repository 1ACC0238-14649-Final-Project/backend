using System;

namespace Gigs.Interfaces.REST.Resources
{
    public class GigResource
    {
        public int Id { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SellerId { get; set; }
        public decimal Price { get; set; }
        public List<string> Tags { get; set; } = new();
        public string Category { get; set; } = string.Empty;
        public int DeliveryDays { get; set; }
        public bool IsResponsive { get; set; }
        public int RevisionCount { get; set; }
        public int PageCount { get; set; }
        public List<string> ExtraFeatures { get; set; } = new();
        public bool CustomAnimations { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}