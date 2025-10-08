namespace Gigs.Domain.Models.Queries
{
    public class GetGigsWithCustomAnimationsQuery
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetGigsWithCustomAnimationsQuery(int page = 1, int pageSize = 10)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
}