using System;
using System.Collections.Generic;

#nullable disable

namespace OHMI_Keeper_League.Models
{
    public partial class Manager
    {
        public Manager()
        {
            Rosters = new HashSet<Roster>();
        }

        public int ManagerId { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual ICollection<Roster> Rosters { get; set; }
    }
}
