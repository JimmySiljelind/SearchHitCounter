namespace SearchHitCounter.Services
{
    public interface ISearchProvider
    {
        string Name { get; }
        Task<long> GetTotalHitsAsync(string query, CancellationToken cancellationToken = default);
    }
}
