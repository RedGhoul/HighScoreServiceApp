using Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sentry;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace HighScoreService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration configuration = null;
            try
            {
                configuration = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .Build();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            using (SentrySdk.Init(Secrets.getConnectionString(configuration, "Sentry_URL")))
            {
               // var elastic = Secrets.getConnectionString(configuration, "ElasticIndexBaseUrl");
               // Log.Logger = new LoggerConfiguration()
               //.Enrich.FromLogContext()
               // .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri($"{elastic}"))
               // {
               //     AutoRegisterTemplate = true,
               //     ModifyConnectionSettings = x => x.BasicAuthentication(Secrets.GetSectionValue(configuration, "AppSettings", "elastic_name"),
               //     Secrets.GetSectionValue(configuration, "AppSettings", "elastic_password")),
               //     AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
               //     IndexFormat = $"{Secrets.GetSectionValue(configuration, "AppSettings", "AppName")}" + "-{0:yyyy.MM}"
               // })
               //.CreateLogger();

                try
                {
                    Log.Information("Starting up");
                    CreateWebHostBuilder(args).Build().Run();
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Application start-up failed");
                }
                finally
                {
                    Log.CloseAndFlush();
                }

            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseStartup<Startup>();
        }
    }
}
