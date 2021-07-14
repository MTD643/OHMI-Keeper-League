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

            _adpCalculator.Calculate("Phillip Rivers");
            _adpCalculator.Calculate("Allen Robinson");
            _adpCalculator.Calculate("Nelson Agholor");
            _adpCalculator.Calculate("David Montgomery");
            _adpCalculator.Calculate("Jonathan Taylor");
            _adpCalculator.Calculate("Noah Fant");
            _adpCalculator.Calculate("JK Dobbins");
            _adpCalculator.Calculate("Harrison Butker");
            _adpCalculator.Calculate("Patrick Mahomes");
            _adpCalculator.Calculate("Rashard Higgins");
            _adpCalculator.Calculate("Devante Parker");
            _adpCalculator.Calculate("Henry Ruggs");
            _adpCalculator.Calculate("Jonnu Smith");
            _adpCalculator.Calculate("Julian Edelman");
            //_adpCalculator.Calculate("Zane Gonzalez");
        }
    }
}
