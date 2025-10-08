using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace Gigs.Interfaces.REST.Resources
{
    public class CreatePullResource
    {
        private static readonly List<string> AllowedStates = new() { "pending", "in_process", "payed", "complete" };

        [Required(ErrorMessage = "Seller ID is required.")]
        public int SellerId { get; set; }

        [Required(ErrorMessage = "Gig ID is required.")]
        public int GigId { get; set; }

        [Required(ErrorMessage = "Initial price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal PriceInit { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Updated price must be greater than 0.")]
        public decimal? PriceUpdate { get; set; }

        public int? BuyerId { get; set; }

        [StringLength(20, ErrorMessage = "State cannot exceed 20 characters.")]
        [RegularExpression("pending|in_process|payed|complete", ErrorMessage = "Invalid state.")]
        public string? State { get; set; }
    }
}