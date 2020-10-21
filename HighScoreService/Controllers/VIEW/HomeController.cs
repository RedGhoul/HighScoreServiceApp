using Application.ViewModels;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HighScoreService.Controllers.VIEW
{
    public class HomeController : Controller
    {
        private readonly ScoreService _scoreService;


        public HomeController(ScoreService bookService)
        {

            _scoreService = bookService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
