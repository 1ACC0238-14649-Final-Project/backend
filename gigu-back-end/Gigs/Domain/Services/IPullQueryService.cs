using Gigs.Domain.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gigs.Domain.Services.QueryServices
{
    public interface IPullQueryService
    {
        Task<IEnumerable<Pull>> GetAllPullsAsync();
        Task<Pull?> GetPullByIdAsync(int id);
        Task<IEnumerable<Pull>> GetPullsByRoleAsync(string role, int userId);
    }
}
