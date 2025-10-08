using System.ComponentModel.DataAnnotations;

namespace Gigs.Domain.Models.Entities
{
    public class Pull
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del vendedor es obligatorio.")]
        public int SellerId { get; set; }
        public int? BuyerId { get; set; }

        [Required(ErrorMessage = "El ID del gig es obligatorio.")]
        public int GigId { get; set; }

        [Required(ErrorMessage = "El precio inicial es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio inicial debe ser mayor que 0.")]
        public decimal PriceInit { get; set; }

        [Required(ErrorMessage = "El precio actualizado es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio actualizado debe ser mayor que 0.")]
        public decimal PriceUpdate { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(20, ErrorMessage = "El estado no debe superar los 20 caracteres.")]
        public string State { get; set; } = "abierta";
        
        public Pull() {}
        
        public Pull(int sellerId, int gigId, decimal priceInit)
        {
            SellerId = sellerId;
            GigId = gigId;
            PriceInit = priceInit;
            PriceUpdate = priceInit;
            State = "abierta";
        }
    }
}
