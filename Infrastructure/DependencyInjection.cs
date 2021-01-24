using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
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

            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseMySql(
                        // Replace with your connection string.
                        AppDBConnectionString,
                        // Replace with your server version and type.
                        // For common usages, see pull request #1233.
                        new MySqlServerVersion(new Version(8, 0, 21)), // use MariaDbServerVersion for MariaDB
                        mySqlOptions => mySqlOptions
                            .CharSetBehavior(CharSetBehavior.NeverAppend))
                    // Everything from this point on is optional but helps with debugging.
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors());

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
