namespace gigu_back_end.Briefcases.Domain.Models.Exceptions;

public class NotProjectFoundException : Exception
{
    public NotProjectFoundException() : base("Not projects found")
    {
    }

    public NotProjectFoundException(string message)
        : base(message)
    {
    }

    public NotProjectFoundException(string message, Exception inner)
        : base(message, inner)
    {
    } 
}