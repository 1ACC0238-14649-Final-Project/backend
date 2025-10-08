namespace gigu_back_end.User.Domain.Models.Exceptions;

public class EmailAlreadyTakenException : Exception
{
    public EmailAlreadyTakenException() : base("Email already taken") { }
}