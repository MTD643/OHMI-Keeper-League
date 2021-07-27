using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using OHMI_Keeper_League.Interfaces;
using OHMI_Keeper_League.Services;
using OHMI_Keeper_League.DAL;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace OHMI_Keeper_League
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddHttpClient()
                .AddScoped<Configurations, Configurations>()
                .AddScoped<ILeagueHistoryService, LeagueHistoryService>()
                .AddScoped<IADPCalculator, ADPCalculator>()
                .AddScoped<IOrchestrator, Orchestrator>()
                .AddScoped<IHttpClientWrapper, HttpClientWrapper>()
                .AddScoped<IDataAccessLayer, DataAccessLayer>()
                .AddDbContext<OHMIKeeperLeagueContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("CONN_STRING")))
                .BuildServiceProvider();

            try
            {
                IOrchestrator orchestrator = serviceProvider.GetService<IOrchestrator>();
                orchestrator.Run();//.GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }


    }
}