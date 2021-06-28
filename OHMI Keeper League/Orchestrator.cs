using OHMI_Keeper_League.Interfaces;
using OHMI_Keeper_League.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace OHMI_Keeper_League
{
    public class Orchestrator : IOrchestrator
    {
        private static ILeagueHistoryService _leagueHistoryService;

        public Orchestrator(ILeagueHistoryService leagueHistoryService)
        {
            _leagueHistoryService = leagueHistoryService;
        }

        public void Run()
        {
            if (Configurations.ShouldRunLeagueHistoryService)
            {
                _leagueHistoryService.AddKeeperValues();
            }
        }
    }
}
