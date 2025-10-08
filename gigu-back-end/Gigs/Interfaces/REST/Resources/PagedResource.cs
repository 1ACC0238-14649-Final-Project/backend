using System.Collections.Generic;

namespace Gigs.Interfaces.REST.Resources
{
    public class PagedResource<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (TotalItems + PageSize - 1) / PageSize;
    }
}