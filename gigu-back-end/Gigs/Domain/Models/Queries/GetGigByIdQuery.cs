namespace Gigs.Domain.Models.Queries
{
    public class GetGigByIdQuery
    {
        public int Id { get; set; }

        public GetGigByIdQuery(int id)
        {
            Id = id;
        }
    }
}
