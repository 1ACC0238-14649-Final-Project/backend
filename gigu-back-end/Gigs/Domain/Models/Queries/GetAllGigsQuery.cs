using System;

namespace Gigs.Domain.Models.Queries
{
    public class GetAllGigsQuery
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchTerm { get; set; } = string.Empty;
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MaxDeliveryDays { get; set; }
        public string SortBy { get; set; } = "CreatedAt";
        public bool Descending { get; set; } = true;

        public GetAllGigsQuery(
            int page = 1, 
            int pageSize = 10, 
            string searchTerm = "",
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int? maxDeliveryDays = null,
            string sortBy = "CreatedAt",
            bool descending = true)
        {
            // Validación básica de parámetros
            if (page < 1) throw new ArgumentException("Page must be greater than 0", nameof(page));
            if (pageSize < 1 || pageSize > 100) throw new ArgumentException("PageSize must be between 1 and 100", nameof(pageSize));
            if (minPrice.HasValue && minPrice < 0) throw new ArgumentException("MinPrice cannot be negative", nameof(minPrice));
            if (maxPrice.HasValue && maxPrice < 0) throw new ArgumentException("MaxPrice cannot be negative", nameof(maxPrice));
            if (maxDeliveryDays.HasValue && maxDeliveryDays < 1) throw new ArgumentException("MaxDeliveryDays must be at least 1", nameof(maxDeliveryDays));

            Page = page;
            PageSize = pageSize;
            SearchTerm = searchTerm;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            MaxDeliveryDays = maxDeliveryDays;
            SortBy = ValidateSortBy(sortBy);
            Descending = descending;
        }

        private static string ValidateSortBy(string sortBy)
        {
            var validSortColumns = new[] { "CreatedAt", "Price", "DeliveryDays", "Title" };
            return validSortColumns.Contains(sortBy) ? sortBy : "CreatedAt";
        }
    }
}