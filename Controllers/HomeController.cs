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

            // Om query är null eller tom, returnera en tom modell
            if (query is not null)
            {
                var trimmed = query.Trim();
                model.Query = trimmed;

                // Om query är tom efter trim, visa ett felmeddelande
                if (string.IsNullOrWhiteSpace(trimmed))
                {
                    model.ErrorMessage = "Please enter at least one word.";
                    return View(model);
                }

                // Hämta sökresultat från tjänsten
                model.Results = await _searchService.GetTotalHitsAsync(trimmed, cancellationToken);
            }

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
