namespace gigu_back_end.Shared.Domain.Models.Commands
{
    public record UpdateUserCommand(string Name, string Lastname, string Email);
}