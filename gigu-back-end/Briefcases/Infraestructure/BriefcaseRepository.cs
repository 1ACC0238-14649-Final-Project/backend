using gigu_back_end.Shared.Domain; 
using gigu_back_end.Briefcases.Domain;
using gigu_back_end.Briefcases.Domain.Models.Entities;
using gigu_back_end.Shared.Infraestructure.Persistence.Repositories;
using gigu_back_end.Shared.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace gigu_back_end.Briefcases.Infraestructure;

public class BriefcaseRepository(GigUContext context)
    : BaseRepository<Briefcase>(context), IBriefcaseRepository
{
    public async Task<Briefcase?> GetByNameAsync(string name)
    {
        return await Context.Set<Briefcase>().FirstOrDefaultAsync(briefcase => briefcase.Name == name);
    }

    public async Task<IEnumerable<Briefcase>> GetAllWithProjectsAsync()
    {
        return await Context.Set<Briefcase>()
            .Include(b => b.Projects)
            .Where(b => b.IsActive)
            .ToListAsync();
    }

    public async Task<Briefcase?> FindBySellerIdWithProjectsAsync(int sellerId)
    {
        return await Context.Set<Briefcase>()
            .Include(b => b.Projects)
            .FirstOrDefaultAsync(b => b.SellerId == sellerId && b.IsActive);
    }
}