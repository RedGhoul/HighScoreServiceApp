using Domain.Entities;
//using HighScoreService.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HighScoreService.Controllers.VIEW
{
    public class LogoutController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly ILogger<LogoutModel> _logger;

        public LogoutController(SignInManager<ApplicationUser> signInManager)//, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            //_logger = logger;
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            //_logger.LogInformation("User logged out.");
            return LocalRedirect("/");
        }
    }
}
