# SearchHitCounter

ASP.NET Core MVC-app som frågar flera sökmotorer och summerar totalt antal träffar per sökterm.

## Förutsättningar

- Visual Studio 2026 med ASP.NET-workload
- API-nycklar för:
  - Google Custom Search (API-nyckel + Search Engine ID)
  - Brave Search API

## Konfigurera API-nycklar

### Alternativ 1:

Redigera `appsettings.json` och fyll i:

```json
"GoogleSearch": {
  "ApiKey": "YOUR_GOOGLE_KEY",
  "SearchEngineId": "YOUR_CSE_ID"
}
```

### Alternativ 2:

- dotnet user-secrets set "GoogleSearch:ApiKey" "YOUR_GOOGLE_KEY"
- dotnet user-secrets set "GoogleSearch:SearchEngineId" "YOUR_CSE_ID"

## Var du skaffar nycklar

- Google Custom Search API: https://console.cloud.google.com/ och https://cse.google.com/cse

## Bygg och kör

- Öppna `SearchHitCounter.csproj` i Visual Studio
- Tryck **F5**
- Navigera till den lokala **https**-adressen och ange en sökfråga
