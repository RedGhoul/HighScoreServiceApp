using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string AppDBConnectionString = "";

            if (configuration.GetValue<string>("Environment").Equals("Dev"))
            {
                AppDBConnectionString = Secrets.getConnectionString(configuration, "HighScoreService_DB_LOCAL");
            }
            else
            {
                AppDBConnectionString = Secrets.getConnectionString(configuration, "HighScoreService_DB_PROD");
            }

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(AppDBConnectionString));

            services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<ScoreDatabaseSettings>(
                configuration.GetSection(nameof(ScoreDatabaseSettings)));

            services.AddSingleton<ScoreDatabaseSettings, ScoreDatabaseSettings>();
            services.AddSingleton<ScoreService>();

            return services;
        }
    }

}
