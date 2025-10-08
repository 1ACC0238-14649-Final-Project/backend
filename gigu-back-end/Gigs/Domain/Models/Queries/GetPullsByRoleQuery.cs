namespace gigu_back_end.Gigs.Domain.Models.Queries
{
    public class GetPullsByRoleQuery
    {
        public string Role { get; set; }
        public int UserId { get; set; }

        public GetPullsByRoleQuery(string role, int userId)
        {
            Role = role;
            UserId = userId;
        }
    }
}