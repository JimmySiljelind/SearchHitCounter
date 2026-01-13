using Microsoft.AspNetCore.Mvc;
using SearchHitCounter.Models;
using SearchHitCounter.Services;
using System.Diagnostics;

namespace SearchHitCounter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISearchService _searchService;

        public HomeController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        // Hämtar sökresultat baserat på användarens sökfråga
        [HttpGet]
        public async Task<IActionResult> Index(string? query, CancellationToken cancellationToken)
        {
            var model = new SearchViewModel();

            // Ingen sökning gjord (första besöket)
            if (!Request.Query.ContainsKey(nameof(query)))
                return View(model);

            // Sökfrågan är tom eller bara mellanslag
            if (string.IsNullOrWhiteSpace(query))
            {
                model.ErrorMessage = "Please enter at least one word.";
                return View(model);
            }

            // Trimma sökfrågan och spara i modellen
            var trimmed = query.Trim();
            model.Query = trimmed;

            // Hämta sökresultat och total träffar från tjänsten
            model.Results = await _searchService.GetTotalHitsAsync(trimmed, cancellationToken);

            return View(model);
        }

        // Hanterar fel och visar en felvy
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
