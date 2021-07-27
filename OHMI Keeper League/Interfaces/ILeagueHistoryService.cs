using OHMI_Keeper_League.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OHMI_Keeper_League.Interfaces
{
    public interface ILeagueHistoryService
    {
        public Task AddKeeperValues();

        public void SubmitKeepers(Manager manager, string playerName, string year);

        public void SubmitManagersDraft(Manager manager, List<string> playersDraftedInOrder, string year);
    }
}
