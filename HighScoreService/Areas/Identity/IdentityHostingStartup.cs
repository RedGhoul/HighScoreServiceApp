using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(HighScoreService.Areas.Identity.IdentityHostingStartup))]
namespace HighScoreService.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}