using OHMI_Keeper_League.Interfaces;
using OHMI_Keeper_League.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OHMI_Keeper_League
{
    public class Orchestrator : IOrchestrator
    {
        private static ILeagueHistoryService _leagueHistoryService;
        private static IADPCalculator _adpCalculator;

        public Orchestrator(ILeagueHistoryService leagueHistoryService, IADPCalculator adpCalculator)
        {
            _leagueHistoryService = leagueHistoryService;
            _adpCalculator = adpCalculator;
        }

        public async Task Run()
        {
            if (Configurations.ShouldRunLeagueHistoryService)
            {
                await _leagueHistoryService.AddKeeperValues();
            }
        }
    }
}
