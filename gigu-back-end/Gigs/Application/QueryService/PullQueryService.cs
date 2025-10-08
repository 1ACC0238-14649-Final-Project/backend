using Gigs.Domain;
using Gigs.Domain.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gigs.Domain.Services.QueryServices;

namespace Gigs.Domain.Services
{
    public class PullQueryService : IPullQueryService
    {
        private readonly IPullRepository _pullRepo;

        public PullQueryService(IPullRepository pullRepo)
        {
            _pullRepo = pullRepo;
        }

        public async Task<IEnumerable<Pull>> GetAllPullsAsync()
        {
            return await _pullRepo.GetAllAsync();
        }

        public async Task<Pull?> GetPullByIdAsync(int id)
        {
            return await _pullRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Pull>> GetPullsByRoleAsync(string role, int userId)
        {
            var pulls = await _pullRepo.GetAllAsync();
            return role.ToLower() switch
            {
                "buyer" => pulls.Where(p => p.BuyerId == userId),
                "seller" => pulls.Where(p => p.SellerId == userId),
                _ => new List<Pull>()
            };
        }
    }
}