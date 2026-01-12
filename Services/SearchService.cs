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

            // Separerar queryn ni i individuella termer
            var terms = query
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            // Om inga termer finns, returnera tom lista
            if (terms.Length == 0)
            {
                return results;
            }

            // Frågar varje provider om total hits för varje term
            foreach (var provider in _providers)
            {
                // Skapar ett resultatobjekt för varje provider
                var result = new SearchResultItem
                {
                    Provider = provider.Name
                };

                try
                {
                    // Summerar total hits för alla termer från providern
                    long total = 0;
                    foreach (var term in terms)
                    {
                        total += await provider.GetTotalHitsAsync(term, cancellationToken);
                    }

                    result.TotalHits = total;
                }
                catch (Exception ex)
                {
                    // Hanterar eventuella fel från providern och loggar felmeddelandet i resultatet
                    result.TotalHits = 0;
                    result.ErrorMessage = ex.Message;
                }

                results.Add(result);
            }

            return results;
        }
    }
}
