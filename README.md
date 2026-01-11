# SearchHitCounter

ASP.NET Core MVC-app som frågar flera sökmotorer och summerar totalt antal träffar per sökterm.

## Förutsättningar

- Visual Studio 2026 med ASP.NET-workload
- API-nycklar för:
  - Google Custom Search (API-nyckel + Search Engine ID)
  - Algolia (Application ID + API key + index name)

## Konfigurera API-nycklar

### Alternativ 1:

Redigera `appsettings.json` och fyll i:

```json
"GoogleSearch": {
  "ApiKey": "YOUR_GOOGLE_KEY",
  "SearchEngineId": "YOUR_CSE_ID"
},
"AlgoliaSearch": {
  "ApplicationId": "YOUR_ALGOLIA_APP_ID",
  "ApiKey": "YOUR_ALGOLIA_API_KEY",
  "IndexName": "YOUR_ALGOLIA_INDEX"
}
```

### Alternativ 2:

- dotnet user-secrets set "GoogleSearch:ApiKey" "YOUR_GOOGLE_KEY"
- dotnet user-secrets set "GoogleSearch:SearchEngineId" "YOUR_CSE_ID"
- dotnet user-secrets set "AlgoliaSearch:ApplicationId" "YOUR_ALGOLIA_APP_ID"
- dotnet user-secrets set "AlgoliaSearch:ApiKey" "YOUR_ALGOLIA_API_KEY"
- dotnet user-secrets set "AlgoliaSearch:IndexName" "YOUR_ALGOLIA_INDEX"

## Var du skaffar nycklar

- Google Custom Search API: https://console.cloud.google.com/ och https://cse.google.com/cse
- Algolia: https://www.algolia.com/

## Bygg och kör

- Öppna `SearchHitCounter.csproj` i Visual Studio
- Tryck **F5**
- Navigera till den lokala **https**-adressen och ange en sökfråga
