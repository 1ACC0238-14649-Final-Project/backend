using Gigs.Domain.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gigs.Domain.Services
{
    public interface IGigDomainService
    {
        
        Task<bool> ValidateGigOwnershipAsync(int gigId, int sellerId);
        
        
        Task<bool> IsCategoryValidAsync(string category);
        
        
        Task<bool> IsUserActiveFreelancerAsync(int sellerId);
        
        
        Task<bool> ValidateGigRequirementsAsync(int gigId);
        
        
        Task<bool> AreTagsValidAsync(IEnumerable<string> tags);
        
        
        Task<bool> ValidateExtraFeaturesAsync(IEnumerable<string> features);
    }
}