using gigu_back_end.Briefcases.Domain.Models.Entities;
using gigu_back_end.Shared.Domain;

namespace gigu_back_end.Briefcases.Domain;

public interface IBriefcaseRepository : IBaseRepository<Briefcase>
{
    Task<Briefcase?> GetByNameAsync(string name);

    Task<IEnumerable<Briefcase>> GetAllWithProjectsAsync();

    Task<Briefcase?> FindBySellerIdWithProjectsAsync(int sellerId); // NUEVO
}
