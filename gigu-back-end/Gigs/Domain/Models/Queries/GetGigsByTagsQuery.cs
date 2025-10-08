namespace Gigs.Domain.Models.Queries
{
    public class GetGigsByTagsQuery
    {
        public IEnumerable<string> Tags { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetGigsByTagsQuery(IEnumerable<string> tags, int page = 1, int pageSize = 10)
        {
            Tags = tags;
            Page = page;
            PageSize = pageSize;
        }
    }
}