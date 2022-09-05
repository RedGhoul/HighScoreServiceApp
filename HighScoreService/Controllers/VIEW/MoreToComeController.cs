using Microsoft.AspNetCore.Mvc;

namespace HighScoreService.Controllers.VIEW;

public class MoreToComeController: Controller
{
    public MoreToComeController()
    {
    }
    
    public IActionResult Index()
    {
        return View();
    }
}