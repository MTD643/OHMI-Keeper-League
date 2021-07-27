using OfficeOpenXml;
using OfficeOpenXml.Table;
using OHMI_Keeper_League.DAL;
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
        private readonly OHMIKeeperLeagueContext _dbContext;
        private static ExcelPackage _package;
        private Dictionary<string, double> _adpLookup;
        private static Tuple<int, int> _adpRangeStart = new Tuple<int, int>(2, 1);
        private static Tuple<int, int> _adpRangeEnd = new Tuple<int, int>(151, 4);

        public ADPCalculator(OHMIKeeperLeagueContext dbContext)
        {
            _dbContext = dbContext;
            FileInfo file = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.GetDirectories("Reports").FirstOrDefault().GetFiles(Configurations.ADPFileName).FirstOrDefault();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _package = new ExcelPackage(file);
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
                Player player = new Player();

                for (int col = _adpRangeStart.Item2; col <= _adpRangeEnd.Item2; col++)
                {
                    //Name
                    if (col == 1)
                    {
                        var value = tab.Cells[row, col].Value;
                        playerName = (value == null ? "" : value.ToString()).CleanupName();

                        string[] splitName = playerName.Split(' ');

                        player.FirstName = splitName[0];
                        player.LastName = splitName[1];

                        if (splitName.Length > 2)
                            player.LastName += $" {splitName[2]}";
                    }

                    //Team
                    if (col == 2)
                    {
                        player.Team = tab.Cells[row, col].Value.ToString();
                    }

                    //Position
                    if (col == 3)
                    {
                        string value = tab.Cells[row, col].Value.ToString();

                        switch (value[0])
                        {
                            case 'Q':
                                player.Position = PlayerPosition.QB.ToString();
                                break;
                            case 'R':
                                player.Position = PlayerPosition.RB.ToString();
                                break;
                            case 'W':
                                player.Position = PlayerPosition.WR.ToString();
                                break;
                            case 'T':
                                player.Position = PlayerPosition.TE.ToString();
                                break;
                            case 'K':
                                player.Position = PlayerPosition.K.ToString();
                                break;
                            case 'P':
                                player.Position = PlayerPosition.K.ToString();
                                break;
                            case 'D':
                                player.Position = PlayerPosition.DST.ToString();
                                break;
                        }
                    }                   

                    //ADP
                    if (col == 4)
                    {
                        var value = tab.Cells[row, col].Value;

                        try
                        {
                            adp = (double)value;
                            player.ADP = adp;
                            player.ProjectedRound = Math.Ceiling(adp / 12);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"PlayerName: {playerName} - {ex}");
                        }
                    }
                }

                if (playerName.HasValue() && adp > 0.0)
                {
                    _adpLookup.TryAdd(playerName, adp);
                    _dbContext.Players.Add(player);
                }
            }

            _dbContext.SaveChanges();
        }

        public string Calculate(string playerName)
        {
            string result = string.Empty;

            if (_adpLookup.TryGetValue(playerName.CleanupName(), out double adp))
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
