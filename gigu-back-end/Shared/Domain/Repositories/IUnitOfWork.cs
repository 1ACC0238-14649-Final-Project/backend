namespace gigu_back_end.Shared.Domain;

public interface IUnitOfWork
{
    Task CompleteAsync();
}