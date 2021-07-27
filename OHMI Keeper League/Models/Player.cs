using System;
using System.Collections.Generic;

#nullable disable

namespace OHMI_Keeper_League.Models
{
    public partial class Player
    {
        public Player()
        {
            RosterB1Navigations = new HashSet<Roster>();
            RosterB2Navigations = new HashSet<Roster>();
            RosterB3Navigations = new HashSet<Roster>();
            RosterB4Navigations = new HashSet<Roster>();
            RosterB5Navigations = new HashSet<Roster>();
            RosterB6Navigations = new HashSet<Roster>();
            RosterB7Navigations = new HashSet<Roster>();
            RosterDstNavigations = new HashSet<Roster>();
            RosterFlexNavigations = new HashSet<Roster>();
            RosterKNavigations = new HashSet<Roster>();
            RosterQb1Navigations = new HashSet<Roster>();
            RosterRb1Navigations = new HashSet<Roster>();
            RosterRb2Navigations = new HashSet<Roster>();
            RosterWr1Navigations = new HashSet<Roster>();
            RosterWr2Navigations = new HashSet<Roster>();
        }

        public int PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Team { get; set; }
        public double? ADP { get; set; }
        public double? ProjectedRound { get; set; }

        public virtual ICollection<Roster> RosterB1Navigations { get; set; }
        public virtual ICollection<Roster> RosterB2Navigations { get; set; }
        public virtual ICollection<Roster> RosterB3Navigations { get; set; }
        public virtual ICollection<Roster> RosterB4Navigations { get; set; }
        public virtual ICollection<Roster> RosterB5Navigations { get; set; }
        public virtual ICollection<Roster> RosterB6Navigations { get; set; }
        public virtual ICollection<Roster> RosterB7Navigations { get; set; }
        public virtual ICollection<Roster> RosterDstNavigations { get; set; }
        public virtual ICollection<Roster> RosterFlexNavigations { get; set; }
        public virtual ICollection<Roster> RosterKNavigations { get; set; }
        public virtual ICollection<Roster> RosterQb1Navigations { get; set; }
        public virtual ICollection<Roster> RosterRb1Navigations { get; set; }
        public virtual ICollection<Roster> RosterRb2Navigations { get; set; }
        public virtual ICollection<Roster> RosterWr1Navigations { get; set; }
        public virtual ICollection<Roster> RosterWr2Navigations { get; set; }
    }
}
