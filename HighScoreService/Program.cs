using System;
using System.Configuration;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Elasticsearch;


var builder = WebApplication.CreateBuilder(args);
var isProd = builder.Configuration.GetValue<string>("Environment").Equals("Prod");
var appDbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var elasticConnectionString = builder.Configuration.GetValue<string>("ElasticURL");
var elasticPassword = builder.Configuration.GetValue<string>("ElasticPassword");

var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticConnectionString))
    {
        IndexFormat =
            $"HighScoreService-logs-{builder.Environment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
        AutoRegisterTemplate = true,
        NumberOfShards = 2,
        NumberOfReplicas = 1,
        ModifyConnectionSettings = x => x.BasicAuthentication("elastic", elasticPassword),
    }).Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .CreateLogger();

builder.Logging.AddSerilog(logger);
builder.Services.AddInfrastructure(builder.Configuration);
if (isProd)
{
    builder.Services.AddRazorPages();
}
else
{
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
}

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddResponseCompression();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "High Score Service", Version = "v1" });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseResponseCompression();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "High Score Service API V1"); });

app.Run();