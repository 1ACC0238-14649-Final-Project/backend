using gigu_back_end.Shared.Infraestructure.Persistence.Repositories;
using Gigs.Domain.Models.Entities;
using Gigs.Domain;
using Microsoft.EntityFrameworkCore;
using gigu_back_end.Shared.Infrastructure.Persistence.Configuration;

namespace Gigs.Infrastructure.Persistence.EFC.Repositories
{
    public class GigRepository : BaseRepository<Gig>, IGigRepository
    {
        public GigRepository(GigUContext context) : base(context) { }

        public async Task<Gig> GetByIdAsync(int id)
        {
            return await Context.Set<Gig>()
                       .FirstOrDefaultAsync(g => g.Id == id)
                   ?? throw new KeyNotFoundException($"Gig with ID {id} not found");
        }

        public async Task<IEnumerable<Gig>> GetAllAsync(
            int page = 1, 
            int pageSize = 10,
            string sortBy = "CreatedAt",
            bool descending = true)
        {
            var query = Context.Set<Gig>().AsNoTracking().AsQueryable();

            query = sortBy switch
            {
                "Price" => descending ? query.OrderByDescending(g => g.Price) : query.OrderBy(g => g.Price),
                "DeliveryDays" => descending ? query.OrderByDescending(g => g.DeliveryDays) : query.OrderBy(g => g.DeliveryDays),
                _ => descending ? query.OrderByDescending(g => g.CreatedAt) : query.OrderBy(g => g.CreatedAt)
            };

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Gig>> GetBySellerIdAsync(int sellerId, int page = 1, int pageSize = 10)
        {
            return await Context.Set<Gig>()
                .AsNoTracking()
                .Where(g => g.SellerId == sellerId)
                .OrderByDescending(g => g.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Gig>> GetByCategoryAsync(
            string category, 
            int page = 1, 
            int pageSize = 10,
            bool? isResponsive = null)
        {
            var query = Context.Set<Gig>()
                .AsNoTracking()
                .Where(g => EF.Functions.Like(g.Category, $"%{category}%"));

            if (isResponsive.HasValue)
            {
                query = query.Where(g => g.IsResponsive == isResponsive.Value);
            }

            return await query
                .OrderByDescending(g => g.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Gig>> SearchAsync(
            string searchTerm, 
            int page = 1, 
            int pageSize = 10,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int? maxDeliveryDays = null)
        {
            var query = Context.Set<Gig>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(g => 
                    EF.Functions.Like(g.Title, $"%{searchTerm}%") ||
                    EF.Functions.Like(g.Description, $"%{searchTerm}%") ||
                    EF.Functions.Like(g.Category, $"%{searchTerm}%"));
            }

            if (minPrice.HasValue)
                query = query.Where(g => g.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(g => g.Price <= maxPrice.Value);

            if (maxDeliveryDays.HasValue)
                query = query.Where(g => g.DeliveryDays <= maxDeliveryDays.Value);

            return await query
                .OrderByDescending(g => g.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Gig>> GetByTagsAsync(IEnumerable<string> tags, int page = 1, int pageSize = 10)
        {
            var tagList = tags.ToList();
            return await Context.Set<Gig>()
                .AsNoTracking()
                .Where(g => tagList.All(t => g.Tags.Contains(t)))
                .OrderByDescending(g => g.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Gig>> GetWithCustomAnimationsAsync(int page = 1, int pageSize = 10)
        {
            return await Context.Set<Gig>()
                .AsNoTracking()
                .Where(g => g.CustomAnimations)
                .OrderByDescending(g => g.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Gig> CreateAsync(Gig gig)
        {
            await AddAsync(gig);
            await Context.SaveChangesAsync();
            return gig;
        }

        public async Task<Gig> UpdateAsync(Gig gig)
        {
            Update(gig);
            await Context.SaveChangesAsync();
            return gig;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var gig = await GetByIdAsync(id);
            Remove(gig);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await Context.Set<Gig>().AnyAsync(g => g.Id == id);
        }

        public async Task<int> CountByCategoryAsync(string category)
        {
            return await Context.Set<Gig>()
                .Where(g => EF.Functions.Like(g.Category, $"%{category}%"))
                .CountAsync();
        }

        public async Task<int> CountBySellerIdAsync(int sellerId)
        {
            return await Context.Set<Gig>()
                .Where(g => g.SellerId == sellerId)
                .CountAsync();
        }

        public async Task<int> CountBySearchCriteriaAsync(
            string? searchTerm = null,
            decimal? minPrice = null,
            decimal? maxPrice = null)
        {
            var query = Context.Set<Gig>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(g => 
                    EF.Functions.Like(g.Title, $"%{searchTerm}%") ||
                    EF.Functions.Like(g.Description, $"%{searchTerm}%"));
            }

            if (minPrice.HasValue)
                query = query.Where(g => g.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(g => g.Price <= maxPrice.Value);

            return await query.CountAsync();
        }

        public async Task<int> CountByTagsAsync(IEnumerable<string> tags)
        {
            var tagList = tags.ToList();
            return await Context.Set<Gig>()
                .Where(g => tagList.Any(t => g.Tags.Contains(t)))
                .CountAsync();
        }

        public async Task<int> CountWithCustomAnimationsAsync()
        {
            return await Context.Set<Gig>()
                .Where(g => g.CustomAnimations)
                .CountAsync();
        }

        [Obsolete("Use CountBySellerIdAsync instead")]
        public async Task<int> CountByUserIdAsync(int userId)
        {
            return await CountBySellerIdAsync(userId);
        }

        [Obsolete("Use GetBySellerIdAsync instead")]
        public async Task<IEnumerable<Gig>> GetByUserIdAsync(int userId)
        {
            return await GetBySellerIdAsync(userId, 1, int.MaxValue);
        }
    }
}