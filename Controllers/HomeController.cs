using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SearchHitCounter.Models;

namespace SearchHitCounter.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index(string? query)
        {
            var model = new SearchViewModel();

            if (query is not null)
            {
                var trimmed = query.Trim();
                model.Query = trimmed;

                if (string.IsNullOrWhiteSpace(trimmed))
                {
                    model.ErrorMessage = "Please enter at least one word.";
                }
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
