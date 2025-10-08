using Gigs.Domain;
using Gigs.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gigs.Domain.Services.CommandServices;
using Gigs.Domain.Services.QueryServices;

namespace Gigs.Domain.Services
{
    public class PullCommandService : IPullCommandService
    {
        private readonly IPullRepository _pullRepo;
        private static readonly HashSet<string> AllowedStates = new()
        {
            "pending", "in_process", "payed", "complete"
        };

        public PullCommandService(IPullRepository pullRepo)
        {
            _pullRepo = pullRepo;
        }

        public async Task OpenPullAsync(Pull pull)
        {
            pull.State = "pending";
            pull.PriceUpdate = pull.PriceInit;
            await _pullRepo.CreateAsync(pull);
            await _pullRepo.SaveChangesAsync();
        }

        public async Task<Pull> UpdatePullAsync(int pullId, decimal? newPrice = null, string? newState = null)
        {
            var pull = await _pullRepo.GetByIdAsync(pullId);
            if (pull == null) throw new Exception("Pull not found");
            if (pull.State == "complete") throw new Exception("The auction is already complete");

            if (newPrice.HasValue && newPrice > 0)
                pull.PriceUpdate = newPrice.Value;

            if (!string.IsNullOrEmpty(newState) && AllowedStates.Contains(newState))
                pull.State = newState;

            await _pullRepo.UpdateAsync(pull);
            await _pullRepo.SaveChangesAsync();

            return pull;
        }

        public async Task<Pull> ClosePullAsync(int pullId)
        {
            var pull = await _pullRepo.GetByIdAsync(pullId);
            if (pull == null) throw new Exception("Pull not found");
            if (pull.State == "complete") throw new Exception("The auction is already complete");

            pull.State = "complete";
            await _pullRepo.UpdateAsync(pull);
            await _pullRepo.SaveChangesAsync();
            return pull;
        }
    }
}
