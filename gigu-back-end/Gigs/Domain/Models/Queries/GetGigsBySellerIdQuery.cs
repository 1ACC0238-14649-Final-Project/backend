namespace Gigs.Domain.Models.Queries
{
    public class GetGigsBySellerIdQuery
    {
        public int SellerId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetGigsBySellerIdQuery(int sellerId, int page = 1, int pageSize = 10)
        {
            SellerId = sellerId;
            Page = page;
            PageSize = pageSize;
        }
    }
}