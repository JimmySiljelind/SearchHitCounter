namespace SearchHitCounter.Services
{
    public interface ISearchService
    {
        Task<IList<SearchResultItem>> GetTotalHitsAsync(string query, CancellationToken cancellationToken = default);
    }
}
