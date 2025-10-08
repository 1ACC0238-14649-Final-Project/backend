using gigu_back_end.Shared.Domain;
using gigu_back_end.Shared.Infrastructure.Persistence.Configuration;

namespace gigu_back_end.Shared.Infraestructure.Persistence.Repositories;

public class UnitOfWork(GigUContext context) : IUnitOfWork
{
    /// <inheritdoc />
    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }
}