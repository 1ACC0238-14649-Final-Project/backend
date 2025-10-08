using Gigs.Domain.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gigs.Domain
{
    public interface IPullRepository
    {
        Task<IEnumerable<Pull>> GetAllAsync();
        Task<Pull> GetByIdAsync(int id);
        Task CreateAsync(Pull pull);
        Task UpdateAsync(Pull pull);
        Task SaveChangesAsync();
    }
}
