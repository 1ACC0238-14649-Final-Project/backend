namespace Gigs.Interfaces.REST.Resources
{
    public class PullResource
    {
        public int Id { get; set; }

        public int SellerId { get; set; }

        public int? BuyerId { get; set; }

        public int GigId { get; set; }

        public decimal PriceInit { get; set; }

        public decimal PriceUpdate { get; set; }

        public string State { get; set; } = "pending";
    }
}