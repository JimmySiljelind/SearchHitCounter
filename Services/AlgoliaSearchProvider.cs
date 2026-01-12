using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace SearchHitCounter.Services
{
    public class AlgoliaSearchProvider : ISearchProvider
    {
        private readonly HttpClient _httpClient;
        private readonly AlgoliaSearchOptions _options;

        public AlgoliaSearchProvider(HttpClient httpClient, IOptions<AlgoliaSearchOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public string Name => "Algolia";

        public async Task<long> GetTotalHitsAsync(string query, CancellationToken cancellationToken = default)
        {
            // Validerar konfigurationsinställningar
            if (string.IsNullOrWhiteSpace(_options.ApplicationId))
            {
                throw new InvalidOperationException("Algolia Application ID is not configured.");
            }

            if (string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                throw new InvalidOperationException("Algolia API key is not configured.");
            }

            if (string.IsNullOrWhiteSpace(_options.IndexName))
            {
                throw new InvalidOperationException("Algolia index name is not configured.");
            }

            if (string.IsNullOrWhiteSpace(_options.EndpointTemplate))
            {
                throw new InvalidOperationException("Algolia endpoint template is not configured.");
            }

            // Konstruerar endpoint URL
            var endpoint = string.Format(
                CultureInfo.InvariantCulture,
                _options.EndpointTemplate,
                _options.ApplicationId,
                Uri.EscapeDataString(_options.IndexName));

            // Skapar HTTP POST-förfrågan
            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("X-Algolia-Application-Id", _options.ApplicationId);
            request.Headers.TryAddWithoutValidation("X-Algolia-API-Key", _options.ApiKey);

            // Skapar JSON-payload med sökfrågan
            var payload = JsonSerializer.Serialize(new { query });
            request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            // Skickar förfrågan och hanterar svaret
            using var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            // Läser och tolkar JSON-svaret
            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            // Extraherar antalet träffar från svaret
            if (document.RootElement.TryGetProperty("nbHits", out var hitsElement)) // "nbHits" är fältet som innehåller antalet träffar
            {
                if (hitsElement.ValueKind == JsonValueKind.Number && hitsElement.TryGetInt64(out var hits))
                {
                    // Hanterar fallet där antalet träffar returneras som ett nummer
                    return hits;
                }

                if (hitsElement.ValueKind == JsonValueKind.String)
                {
                    // Hanterar fallet där antalet träffar returneras som en sträng
                    var hitsText = hitsElement.GetString();
                    if (long.TryParse(hitsText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedHits))
                    {
                        return parsedHits;
                    }
                }
            }

            return 0;
        }
    }
}
