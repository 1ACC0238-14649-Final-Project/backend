namespace gigu_back_end.Shared.Domain.Models.Commands;

public record CreateUserCommand(string Name, string Lastname, string Email, string Password, string Role, string Image);