using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gigs.Interfaces.REST.Resources
{
    public class UpdateGigResource
    {
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
        public string Category { get; set; } = string.Empty;

        [Range(1, 365, ErrorMessage = "Delivery days must be between 1 and 365")]
        public int DeliveryDays { get; set; }

        public List<string> Tags { get; set; } = new();

        public bool? IsResponsive { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Revision count cannot be negative")]
        public int? RevisionCount { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page count must be at least 1")]
        public int? PageCount { get; set; }

        public List<string> ExtraFeatures { get; set; } = new();

        public bool? CustomAnimations { get; set; }
    }
}