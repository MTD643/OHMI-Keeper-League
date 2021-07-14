using OfficeOpenXml;
using OfficeOpenXml.Table;
using OHMI_Keeper_League.Interfaces;
using OHMI_Keeper_League.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OHMI_Keeper_League.Enums.Enums;

namespace OHMI_Keeper_League.Services
{
    public class ADPCalculator : IADPCalculator
    {
        private static ExcelPackage _package;
        private Dictionary<string, double> _adpLookup;
        private static Tuple<int, int> _adpRangeStart = new Tuple<int, int>(2, 2);
        private static Tuple<int, int> _adpRangeEnd = new Tuple<int, int>(395, 10);

        public ADPCalculator()
        {
            FileInfo file = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.GetDirectories("Reports").FirstOrDefault().GetFiles(Configurations.ADPFileName).FirstOrDefault();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _package = new ExcelPackage(file);

            LoadADPSheet();
        }

        void LoadADPSheet()
        {
            if (_adpLookup == null)
            {
                _adpLookup = new Dictionary<string, double>();
            }

            ExcelWorksheet tab = _package.Workbook.Worksheets[0];

            for (int row = _adpRangeStart.Item1; row <= _adpRangeEnd.Item1; row++)
            {
                string playerName = string.Empty;
                double adp = 0.0;

                for (int col = _adpRangeStart.Item2; col <= _adpRangeEnd.Item2; col++)
                {
                    if (col == 2)
                    {
                        var value = tab.Cells[row, col].Value;
                        playerName = (value == null ? "" : value.ToString()).CleanupName();
                    }

                    if (col == 10)
                    {
                        var value = tab.Cells[row, col].Value;
                        try
                        {
                            adp = (double) value;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"PlayerName: {playerName} - {ex}");
                        }
                    }

                    if (playerName.HasValue() && adp > 0.0)
                    {
                        _adpLookup.TryAdd(playerName, adp);
                    }
                }
            }
        }

        public string Calculate(string playerName)
        {
            string result = string.Empty;

            if(_adpLookup.TryGetValue(playerName.CleanupName(), out double adp))
            {
                result = $"{playerName}\r\nADP: {adp}\r\nProj. Round: {Math.Ceiling(adp / 12)}";
            }
            else
            {
                result = $"{playerName}\r\nProbably shouldn't draft him...";
            }

            result += "\r\n";

            Console.WriteLine(result);

            return result;
        }
    }
}
