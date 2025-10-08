namespace gigu_back_end.User.Domain.Models.Exceptions;

public class NotRequiredFieldsCompletedException : Exception
{
    public NotRequiredFieldsCompletedException() : base("Not all required fields are completed.") { }
    public NotRequiredFieldsCompletedException(string message) : base(message) { }
    public NotRequiredFieldsCompletedException(string message, Exception inner) : base(message, inner) { }
}