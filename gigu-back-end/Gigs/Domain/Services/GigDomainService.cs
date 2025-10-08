using Gigs.Domain.Models.Entities;
using Gigs.Domain;
using Gigs.Domain.Services;
using System;
using System.Threading.Tasks;
using gigu_back_end.User.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Gigs.Domain.Services
{
    public class GigDomainService : IGigDomainService
    {
        private readonly IGigRepository _gigRepository;
        private readonly IUserRepository _userRepository;
        private static readonly HashSet<string> _validCategories = new(StringComparer.OrdinalIgnoreCase)
        {
            "Programming", "Design", "Writing", "Marketing", "Video", "Music", "Web Development"
        };

        private static readonly HashSet<string> _allowedFeatures = new(StringComparer.OrdinalIgnoreCase)
        {
            "SEO optimization", "Performance tuning", "Mobile responsive", 
            "CMS integration", "E-commerce", "Multi-language"
        };

        public GigDomainService(IGigRepository gigRepository, IUserRepository userRepository)
        {
            _gigRepository = gigRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> ValidateGigOwnershipAsync(int gigId, int sellerId)
        {
            var gig = await _gigRepository.GetByIdAsync(gigId);
            return gig?.SellerId == sellerId;
        }

        public Task<bool> IsCategoryValidAsync(string category)
        {
            return Task.FromResult(_validCategories.Contains(category));
        }

        public async Task<bool> IsUserActiveFreelancerAsync(int sellerId)
        {
            var user = await _userRepository.FindByIdAsync(sellerId);
            return user != null && user.IsActive && user.Role == "Freelancer";
        }

        public async Task<bool> ValidateGigRequirementsAsync(int gigId)
        {
            var gig = await _gigRepository.GetByIdAsync(gigId);
            if (gig == null) return false;

            return gig.PageCount > 0 && 
                   gig.DeliveryDays > 0 && 
                   !string.IsNullOrWhiteSpace(gig.Image);
        }

        public Task<bool> AreTagsValidAsync(IEnumerable<string> tags)
        {
            return Task.FromResult(tags == null || tags.All(t => 
                !string.IsNullOrWhiteSpace(t) && t.Length <= 50));
        }

        public Task<bool> ValidateExtraFeaturesAsync(IEnumerable<string> features)
        {
            // Validación 1: Máximo 5 features
            if (features?.Count() > 5) 
                return Task.FromResult(false);

            // Validación 2: Todos deben estar en la lista permitida
            return Task.FromResult(features == null || 
                   features.All(f => _allowedFeatures.Contains(f)));
        }
    }
}