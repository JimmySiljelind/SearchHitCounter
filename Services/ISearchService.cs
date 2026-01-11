using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SearchHitCounter.Models;

namespace SearchHitCounter.Services
{
    public interface ISearchService
    {
        Task<IList<SearchResultItem>> GetTotalHitsAsync(string query, CancellationToken cancellationToken = default);
    }
}
