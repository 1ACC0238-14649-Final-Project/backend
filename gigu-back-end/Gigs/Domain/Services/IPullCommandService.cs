using Gigs.Domain.Models.Entities;
using System.Threading.Tasks;
namespace Gigs.Domain.Services.CommandServices
{
    public interface IPullCommandService
    {
        Task OpenPullAsync(Pull pull);
        Task<Pull> UpdatePullAsync(int pullId, decimal? newPrice = null, string? newState = null);
        Task<Pull> ClosePullAsync(int pullId);
    }
}