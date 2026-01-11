using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace SearchHitCounter.Services
{
    public class GoogleSearchProvider : ISearchProvider
    {
        private readonly HttpClient _httpClient;
        private readonly GoogleSearchOptions _options;

        public GoogleSearchProvider(HttpClient httpClient, IOptions<GoogleSearchOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public string Name => "Google";

        public async Task<long> GetTotalHitsAsync(string query, CancellationToken cancellationToken = default)
        {
            // Validera konfigurationen
            if (string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                throw new InvalidOperationException("Google Custom Search API key is not configured.");
            }
            if (string.IsNullOrWhiteSpace(_options.SearchEngineId))
            {
                throw new InvalidOperationException("Google Custom Search engine ID is not configured.");
            }
            if (string.IsNullOrWhiteSpace(_options.Endpoint))
            {
                throw new InvalidOperationException("Google Custom Search endpoint is not configured.");
            }

            // Bygg förfrågnings-URI
            var requestUri =
                $"{_options.Endpoint}?key={Uri.EscapeDataString(_options.ApiKey)}&cx={Uri.EscapeDataString(_options.SearchEngineId)}&q={Uri.EscapeDataString(query)}";

            // Skicka förfrågan och hantera svaret
            using var response = await _httpClient.GetAsync(requestUri, cancellationToken);
            response.EnsureSuccessStatusCode();

            // Läs och analysera JSON-svaret
            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            // (searchInformation.totalResults) Det uppskattade totala antalet träffar över alla sidor för den sökfrågan.
            if (document.RootElement.TryGetProperty("searchInformation", out var info)
                && info.TryGetProperty("totalResults", out var totalResults))
            {
                var totalString = totalResults.GetString();
                if (long.TryParse(totalString, NumberStyles.Integer, CultureInfo.InvariantCulture, out var total))
                {
                    return total;
                }
            }

            return 0;
        }
    }
}
