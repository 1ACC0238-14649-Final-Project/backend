using Gigs.Domain.Models.Entities;


namespace Gigs.Domain
{
    public interface IGigRepository
    {
        // Operaciones básicas CRUD
        Task<Gig> GetByIdAsync(int id);
        Task<Gig> CreateAsync(Gig gig);
        Task<Gig> UpdateAsync(Gig gig);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        // Consultas avanzadas
        Task<IEnumerable<Gig>> GetAllAsync(
            int page = 1, 
            int pageSize = 10,
            string sortBy = "CreatedAt",
            bool descending = true);

        Task<IEnumerable<Gig>> GetBySellerIdAsync(int sellerId, int page = 1, int pageSize = 10); 
        
        Task<IEnumerable<Gig>> GetByCategoryAsync(
            string category, 
            int page = 1, 
            int pageSize = 10,
            bool? isResponsive = null);
        
        Task<IEnumerable<Gig>> SearchAsync(
            string searchTerm, 
            int page = 1, 
            int pageSize = 10,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int? maxDeliveryDays = null);
        
        // Métodos para características específicas
        Task<IEnumerable<Gig>> GetByTagsAsync(IEnumerable<string> tags, int page = 1, int pageSize = 10);
        Task<IEnumerable<Gig>> GetWithCustomAnimationsAsync(int page = 1, int pageSize = 10);
        
        // Métodos de conteo
        Task<int> CountByCategoryAsync(string category);
        Task<int> CountBySellerIdAsync(int sellerId);
        Task<int> CountBySearchCriteriaAsync(
            string? searchTerm = null,
            decimal? minPrice = null,
            decimal? maxPrice = null);
        Task<int> CountByTagsAsync(IEnumerable<string> tags);
        Task<int> CountWithCustomAnimationsAsync();
        
        // Métodos obsoletos (para compatibilidad)
        [System.Obsolete("Use GetBySellerIdAsync instead")]
        Task<IEnumerable<Gig>> GetByUserIdAsync(int userId);
        
        [System.Obsolete("Use CountBySellerIdAsync instead")]
        Task<int> CountByUserIdAsync(int userId);
    }
}