using System;
using System.Collections.Generic;
using System.Text;

namespace OHMI_Keeper_League.Enums
{
    public class Enums
    {
        public enum Manager
        {
            MICHAELDONEY,
            TOMDONEY,
            GAILDONEY,
            LUNDENCARPENTER,
            TORINCARPENTER,
            ROGERSAMSON,
            JENNYJACOB,
            GEORGEJACOB,
            ANDREWGIZA,
            THOMASMARINE,
            BRIENGARVEY,
            BOBBYWILLEN
        }

        public enum ApiType
        {
            Post,
            Get,
            Delete,
            Put,
            Patch
        }

        public enum PlayerPosition
        {
            QB,
            RB,
            WR,
            TE,
            K,
            DST
        }
    }
}
