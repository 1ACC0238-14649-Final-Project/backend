namespace gigu_back_end.User.Domain.Models.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base("Invalid email or password") { }
}