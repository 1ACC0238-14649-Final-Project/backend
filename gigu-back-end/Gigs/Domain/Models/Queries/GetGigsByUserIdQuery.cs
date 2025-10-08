namespace Gigs.Domain.Models.Queries
{
    public class GetGigsByUserIdQuery
    {
        public int UserId { get; set; }

        public GetGigsByUserIdQuery(int userId)
        {
            UserId = userId;
        }
    }
}