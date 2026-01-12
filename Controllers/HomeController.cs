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

        [HttpGet]
        public async Task<IActionResult> Index(string? query, CancellationToken cancellationToken)
        {
            var model = new SearchViewModel();

            if (query is not null)
            {
                var trimmed = query.Trim();
                model.Query = trimmed;

                if (string.IsNullOrWhiteSpace(trimmed))
                {
                    model.ErrorMessage = "Please enter at least one word.";
                    return View(model);
                }

                model.Results = await _searchService.GetTotalHitsAsync(trimmed, cancellationToken);
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
