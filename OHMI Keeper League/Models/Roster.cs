using System;
using System.Collections.Generic;

#nullable disable

namespace OHMI_Keeper_League.Models
{
    public partial class Roster
    {
        public int RosterId { get; set; }
        public int ManagerId { get; set; }
        public int Year { get; set; }
        public int? Qb1 { get; set; }
        public int? Rb1 { get; set; }
        public int? Rb2 { get; set; }
        public int? Wr1 { get; set; }
        public int? Wr2 { get; set; }
        public int? Flex { get; set; }
        public int? K { get; set; }
        public int? Dst { get; set; }
        public int? B1 { get; set; }
        public int? B2 { get; set; }
        public int? B3 { get; set; }
        public int? B4 { get; set; }
        public int? B5 { get; set; }
        public int? B6 { get; set; }
        public int? B7 { get; set; }

        public virtual Player B1Navigation { get; set; }
        public virtual Player B2Navigation { get; set; }
        public virtual Player B3Navigation { get; set; }
        public virtual Player B4Navigation { get; set; }
        public virtual Player B5Navigation { get; set; }
        public virtual Player B6Navigation { get; set; }
        public virtual Player B7Navigation { get; set; }
        public virtual Player DstNavigation { get; set; }
        public virtual Player FlexNavigation { get; set; }
        public virtual Player KNavigation { get; set; }
        public virtual Manager Manager { get; set; }
        public virtual Player Qb1Navigation { get; set; }
        public virtual Player Rb1Navigation { get; set; }
        public virtual Player Rb2Navigation { get; set; }
        public virtual Player Wr1Navigation { get; set; }
        public virtual Player Wr2Navigation { get; set; }
    }
}
