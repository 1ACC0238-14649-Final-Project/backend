namespace gigu_back_end.User.Domain.Models.Commands;

public record GoogleLoginCommand(
    string IdToken,
    string? Email = null,
    string? Name = null,
    string? Image = null
);

