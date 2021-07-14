using System;
using System.Collections.Generic;
using System.Text;

namespace OHMI_Keeper_League
{
    public static class StringExtensions
    {
        public static bool HasValue(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        public static string CleanupName(this string str)
        {
            return str.Replace("'", "").Replace(" III", "").Replace(" II", "").Replace(" IV", "").Replace(" Jr.", "").Replace(" Sr.", "").Replace(".", "").Replace("-", "").Trim().ToUpper();
        }
    }
}
