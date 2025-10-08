namespace gigu_back_end.User.Domain.Models.Commands
{
    public record GetUserByIdQuery
    {
        public GetUserByIdQuery(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; init; }
    }
}