namespace SearchHitCounter.Services
{
    public class AlgoliaSearchOptions
    {
        public string ApplicationId { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string IndexName { get; set; } = string.Empty;
        public string EndpointTemplate { get; set; } = "https://{0}-dsn.algolia.net/1/indexes/{1}/query";
    }
}
