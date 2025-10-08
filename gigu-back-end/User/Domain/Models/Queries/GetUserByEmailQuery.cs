namespace gigu_back_end.User.Domain.Models.Commands
{
    public record GetUserByEmailQuery
    {
        public GetUserByEmailQuery(string userEmail)
        {
            UserEmail = userEmail;
        }

        public string UserEmail { get; init; }
    }
}