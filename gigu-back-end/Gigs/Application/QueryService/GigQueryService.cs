using Gigs.Domain.Models.Entities;
using Gigs.Domain.Models.Exceptions;
using Gigs.Domain.Models.Queries;
using Gigs.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gigs.Application.QueryService
{
    public class GigQueryService
    {
        private readonly IGigRepository _gigRepository;

        public GigQueryService(IGigRepository gigRepository)
        {
            _gigRepository = gigRepository;
        }

        public async Task<Gig> GetGigByIdAsync(GetGigByIdQuery query)
        {
            var gig = await _gigRepository.GetByIdAsync(query.Id);
            return gig ?? throw new GigNotFoundException(query.Id);
        }

        public async Task<(IEnumerable<Gig> gigs, int totalCount)> GetAllGigsAsync(GetAllGigsQuery query)
        {
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                var gigs = await _gigRepository.SearchAsync(
                    query.SearchTerm, 
                    query.Page, 
                    query.PageSize,
                    query.MinPrice,
                    query.MaxPrice,
                    query.MaxDeliveryDays);

                var total = await _gigRepository.CountBySearchCriteriaAsync(
                    query.SearchTerm,
                    query.MinPrice,
                    query.MaxPrice);

                return (gigs, total);
            }
            
            var allGigs = await _gigRepository.GetAllAsync(
                query.Page, 
                query.PageSize,
                query.SortBy,
                query.Descending);

            var totalCount = await _gigRepository.CountBySearchCriteriaAsync();
            return (allGigs, totalCount);
        }

        public async Task<(IEnumerable<Gig> gigs, int totalCount)> GetGigsBySellerIdAsync(GetGigsBySellerIdQuery query)
        {
            var gigs = await _gigRepository.GetBySellerIdAsync(query.SellerId);
            var total = await _gigRepository.CountBySellerIdAsync(query.SellerId);
            return (gigs, total);
        }

        public async Task<(IEnumerable<Gig> gigs, int totalCount)> GetGigsByCategoryAsync(GetGigsByCategoryQuery query)
        {
            var gigs = await _gigRepository.GetByCategoryAsync(
                query.Category, 
                query.Page, 
                query.PageSize,
                query.IsResponsive);

            var total = await _gigRepository.CountByCategoryAsync(query.Category);
            return (gigs, total);
        }

        public async Task<(IEnumerable<Gig> gigs, int totalCount)> GetGigsByTagsAsync(GetGigsByTagsQuery query)
        {
            var gigs = await _gigRepository.GetByTagsAsync(
                query.Tags, 
                query.Page, 
                query.PageSize);

            // Nota: Asumiendo que necesitas implementar CountByTagsAsync en el repositorio
            var total = await _gigRepository.CountBySearchCriteriaAsync(); // Temporal
            return (gigs, total);
        }

        public async Task<(IEnumerable<Gig> gigs, int totalCount)> GetWithCustomAnimationsAsync(GetGigsWithCustomAnimationsQuery query)
        {
            var gigs = await _gigRepository.GetWithCustomAnimationsAsync(
                query.Page, 
                query.PageSize);

            // Nota: Asumiendo que necesitas implementar CountWithCustomAnimationsAsync en el repositorio
            var total = await _gigRepository.CountBySearchCriteriaAsync(); // Temporal
            return (gigs, total);
        }

        public async Task<bool> GigExistsAsync(int gigId)
        {
            return await _gigRepository.ExistsAsync(gigId);
        }

        public async Task<int> GetGigCountByCategoryAsync(string category)
        {
            return await _gigRepository.CountByCategoryAsync(category);
        }

        public async Task<int> GetGigCountBySellerIdAsync(int sellerId)
        {
            return await _gigRepository.CountBySellerIdAsync(sellerId);
        }

        public async Task<int> GetGigCountBySearchCriteriaAsync(
            string? searchTerm = null,
            decimal? minPrice = null,
            decimal? maxPrice = null)
        {
            return await _gigRepository.CountBySearchCriteriaAsync(
                searchTerm,
                minPrice,
                maxPrice);
        }

        // Métodos obsoletos para compatibilidad
        #region Obsolete Methods
        [Obsolete("Use GetGigsBySellerIdAsync instead")]
        public async Task<IEnumerable<Gig>> GetGigsByUserIdAsync(GetGigsByUserIdQuery query)
        {
            return await _gigRepository.GetByUserIdAsync(query.UserId);
        }

        [Obsolete("Use GetGigCountBySellerIdAsync instead")]
        public async Task<int> GetGigCountByUserIdAsync(int userId)
        {
            return await _gigRepository.CountByUserIdAsync(userId);
        }
        #endregion
    }
}