using System.ComponentModel.DataAnnotations;

namespace Gigs.Interfaces.REST.Resources
{
    public class SavePullResource
    {
        [Required(ErrorMessage = "El ID del vendedor es obligatorio.")]
        public int SellerId { get; set; }

        [Required(ErrorMessage = "El ID del gig es obligatorio.")]
        public int GigId { get; set; }

        [Required(ErrorMessage = "El precio inicial es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio inicial debe ser mayor que 0.")]
        public decimal PriceInit { get; set; }

        // ✅ Hacemos PriceUpdate opcional, y si no se proporciona, se usa PriceInit
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio actualizado debe ser mayor que 0.")]
        public decimal? PriceUpdate { get; set; }

        // ✅ BuyerId es opcional, hasta que se cierre la subasta
        public int? BuyerId { get; set; }

        // ✅ Estado opcional, por defecto será "abierta"
        [StringLength(20)]
        public string? State { get; set; }
    }
}
