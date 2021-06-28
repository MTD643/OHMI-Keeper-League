using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using OHMI_Keeper_League.Interfaces;
using OHMI_Keeper_League.Services;
using System;
using System.IO;

namespace OHMI_Keeper_League
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddScoped<Configurations, Configurations>()
                .AddScoped<ILeagueHistoryService, LeagueHistoryService>()
                .AddScoped<IOrchestrator, Orchestrator>()
                .BuildServiceProvider();

            try
            {
                IOrchestrator orchestrator = serviceProvider.GetService<IOrchestrator>();
                orchestrator.Run();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }


    }
}