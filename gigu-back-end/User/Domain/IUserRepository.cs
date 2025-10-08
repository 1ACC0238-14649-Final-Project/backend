using gigu_back_end.Shared.Domain;
using gigu_back_end.User.Domain.Models.Entities;

namespace gigu_back_end.User.Domain;

public interface IUserRepository
{
    Task<Models.Entities.User?> FindByEmailAsync(string email);
    Task<Models.Entities.User?> FindByIdAsync(int id);
    Task<IEnumerable<Models.Entities.User>> ListAsync();
    Task AddAsync(Models.Entities.User user);
    void Update(Models.Entities.User user);
    Task<Domain.Models.Entities.User?> GetByEmailAsync(string email);

}