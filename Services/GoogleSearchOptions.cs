namespace SearchHitCounter.Services
{
    public class GoogleSearchOptions
    {
        public string Endpoint { get; set; } = "https://www.googleapis.com/customsearch/v1";
        public string ApiKey { get; set; } = string.Empty;
        public string SearchEngineId { get; set; } = string.Empty;
    }
}
