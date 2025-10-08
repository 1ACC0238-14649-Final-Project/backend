using gigu_back_end.Shared.Domain;
using gigu_back_end.Shared.Infraestructure.Persistence.Repositories;
using gigu_back_end.Shared.Infrastructure.Persistence.Configuration;
using gigu_back_end.User.Domain;
using gigu_back_end.User.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace gigu_back_end.User.Infrastructure.Persistence.EFC.Repositories;

public class UserRepository(GigUContext context) : BaseRepository<Domain.Models.Entities.User>(context), IUserRepository
{
    public async Task<Domain.Models.Entities.User?> FindByEmailAsync(string email)
    {
        return await Context.Set<Domain.Models.Entities.User>().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Domain.Models.Entities.User?> FindByIdAsync(int id)
    {
        return await Context.Set<Domain.Models.Entities.User>().FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<Domain.Models.Entities.User>> ListAsync()
    {
        return await Context.Set<Domain.Models.Entities.User>().ToListAsync();
    }

    public async Task AddAsync(Domain.Models.Entities.User user)
    {
        await Context.Set<Domain.Models.Entities.User>().AddAsync(user);
    }

    public void Update(Domain.Models.Entities.User user)
    {
        Context.Set<Domain.Models.Entities.User>().Update(user);
    }
    public async Task<Domain.Models.Entities.User?> GetByEmailAsync(string email)
    {
        return await Context.Set<Domain.Models.Entities.User>().FirstOrDefaultAsync(u => u.Email == email);
    }
}