using SearchHitCounter.Models;

namespace SearchHitCounter.Services
{
    public class SearchService : ISearchService
    {
        private readonly IEnumerable<ISearchProvider> _providers;

        public SearchService(IEnumerable<ISearchProvider> providers)
        {
            _providers = providers;
        }

        public async Task<IList<SearchResultItem>> GetTotalHitsAsync(string query, CancellationToken cancellationToken = default)
        {
            var results = new List<SearchResultItem>();

            var terms = query
                .Split(' ', System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries);

            if (terms.Length == 0)
            {
                return results;
            }

            foreach (var provider in _providers)
            {
                var result = new SearchResultItem
                {
                    Provider = provider.Name
                };

                try
                {
                    long total = 0;
                    foreach (var term in terms)
                    {
                        total += await provider.GetTotalHitsAsync(term, cancellationToken);
                    }

                    result.TotalHits = total;
                }
                catch (System.Exception ex)
                {
                    result.TotalHits = 0;
                    result.ErrorMessage = ex.Message;
                }

                results.Add(result);
            }

            return results;
        }
    }
}
