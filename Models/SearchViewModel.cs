using System.Collections.Generic;

namespace SearchHitCounter.Models
{
    public class SearchViewModel
    {
        public string? Query { get; set; }
        public string? ErrorMessage { get; set; }
        public IList<SearchResultItem> Results { get; set; } = new List<SearchResultItem>();
    }

    public class SearchResultItem
    {
        public string Provider { get; set; } = string.Empty;
        public long TotalHits { get; set; }
    }
}
