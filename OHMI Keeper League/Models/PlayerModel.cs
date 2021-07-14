using System;
using System.Collections.Generic;
using System.Text;
using static OHMI_Keeper_League.Enums.Enums;

namespace OHMI_Keeper_League.Models
{
    public class PlayerModel
    {
        public string Name { get; set; }
        public int RoundDrafted { get; set; }
        public Manager OwnedBy { get; set; }
        public string DraftYear { get; set; }
        public bool IsKeeper { get; set; }
        public double ADP { get; set; }
        public int YearsKeptByCurrentManager { get; set; }

        public PlayerModel(string playerName, int roundDrafted, string managerName, string draftYear)
        {
            this.Name = playerName;
            this.RoundDrafted = roundDrafted;
            this.OwnedBy = (Manager)Enum.Parse(typeof(Manager), managerName.Replace(" ", "").ToUpper());
            this.DraftYear = draftYear;
        }
    }
}
