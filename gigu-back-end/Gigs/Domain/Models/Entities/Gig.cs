using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gigs.Domain.Models.Entities
{
    public class Gig
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Image { get; set; } = string.Empty; // Cambiado a string para base64

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int SellerId { get; set; } // Cambiado de UserId a SellerId

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public List<string> Tags { get; set; } = new(); // Nueva propiedad

        [Required]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [Range(1, 365)]
        public int DeliveryDays { get; set; }

        public bool IsResponsive { get; set; } // Nueva propiedad

        public int RevisionCount { get; set; } = 3; // Nueva propiedad con valor por defecto

        public int PageCount { get; set; } // Nueva propiedad

        public List<string> ExtraFeatures { get; set; } = new(); // Nueva propiedad

        public bool CustomAnimations { get; set; } // Nueva propiedad

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Constructor actualizado
        public Gig(string image, string title, string description, int sellerId, 
            decimal price, List<string> tags, string category, int deliveryDays)
        {
            Image = image;
            Title = title;
            Description = description;
            SellerId = sellerId;
            Price = price;
            Tags = tags;
            Category = category;
            DeliveryDays = deliveryDays;
        }
    }
}