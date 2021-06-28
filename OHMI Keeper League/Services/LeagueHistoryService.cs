using OfficeOpenXml;
using OfficeOpenXml.Table;
using OHMI_Keeper_League.Interfaces;
using OHMI_Keeper_League.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static OHMI_Keeper_League.Enums.Enums;

namespace OHMI_Keeper_League.Services
{
    public class LeagueHistoryService : ILeagueHistoryService
    {
        private static Configurations _config;
        private static ExcelPackage _package;
        private Dictionary<string, Dictionary<string, PlayerModel>> _draftBoardsByYear;
        private Dictionary<string, Dictionary<int, List<string>>> _finalRostersByYear;
        private static Tuple<int, int> _keeperRangeStart = new Tuple<int, int>(19, 1);
        private static Tuple<int, int> _keeperRangeEnd = new Tuple<int, int>(21, 12);

        public LeagueHistoryService(Configurations config)
        {
            _config = config;

            Setup();
        }

        void Setup()
        {
            FileInfo file = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.GetDirectories("Reports").FirstOrDefault().GetFiles(Configurations.FileName).FirstOrDefault();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _package = new ExcelPackage(file);

            GetDraftBoards();
            GetFinalRosters();
        }

        void GetDraftBoards()
        {
            if (_draftBoardsByYear == null)
            {
                _draftBoardsByYear = new Dictionary<string, Dictionary<string, PlayerModel>>();
            }

            foreach (ExcelWorksheet tab in _package.Workbook.Worksheets)
            {
                Dictionary<string, PlayerModel> currentDraftBoard = new Dictionary<string, PlayerModel>();
                ExcelTable draftTable = tab.Tables[0];
                ExcelCellAddress start = draftTable.Range.Start;
                ExcelCellAddress end = draftTable.Range.End;
                string year = tab.Name;
                int roundNumber = 0;

                for (int row = start.Row + 1; row <= end.Row; row++)
                {
                    roundNumber++;
                    for (int managerNumber = start.Column; managerNumber <= end.Column; managerNumber++)
                    {
                        var value = tab.Cells[row, managerNumber].Value;
                        string playerName = (value == null ? "" : value.ToString());

                        if (!string.IsNullOrWhiteSpace(playerName))
                        {
                            if (playerName.Contains("("))
                            {
                                playerName = playerName.Split(" (")[0];
                            }

                            PlayerModel playerModel = new PlayerModel(playerName, roundNumber, tab.Cells[2, managerNumber].Value.ToString(), year);

                            currentDraftBoard.Add(playerName, playerModel);
                        }
                    }
                }

                _draftBoardsByYear.Add(year, currentDraftBoard);
            }
        }

        void GetFinalRosters()
        {
            if (_finalRostersByYear == null)
            {
                _finalRostersByYear = new Dictionary<string, Dictionary<int, List<string>>>();
            }

            foreach (ExcelWorksheet tab in _package.Workbook.Worksheets)
            {
                Dictionary<int, List<string>> currentFinalRoster = new Dictionary<int, List<string>>();
                ExcelTable finalRosterTable = tab.Tables[1];
                ExcelCellAddress start = finalRosterTable.Range.Start;
                ExcelCellAddress end = finalRosterTable.Range.End;
                string year = tab.Name;

                for (int roundNumber = start.Row + 1; roundNumber <= end.Row; roundNumber++)
                {
                    List<string> playersDrafted = new List<string>();

                    for (int managerNumber = start.Column; managerNumber <= end.Column; managerNumber++)
                    {
                        var value = tab.Cells[roundNumber, managerNumber].Value;
                        string playerName = (value == null ? "" : value.ToString());

                        playersDrafted.Add(playerName);

                        if (!string.IsNullOrWhiteSpace(playerName))
                        {
                            if (playerName.Contains("("))
                            {
                                tab.Cells[roundNumber, managerNumber].Value = tab.Cells[roundNumber, managerNumber].Value.ToString().Split(" (")[0];
                                playerName = tab.Cells[roundNumber, managerNumber].Value.ToString();
                            }

                            Dictionary<string, PlayerModel> currentDraftBoard = _draftBoardsByYear[year];

                            if (currentDraftBoard.TryGetValue(playerName, out PlayerModel playerModel))
                            {
                                if (playerModel.RoundDrafted > 2)
                                {
                                    tab.Cells[roundNumber, managerNumber].Value += $" ({playerModel.RoundDrafted - 1})";
                                }
                                else
                                {
                                    tab.Cells[roundNumber, managerNumber].Value += $" (NA)";
                                }
                            }
                            else
                            {
                                tab.Cells[roundNumber, managerNumber].Value += $" (15)";
                            }
                        }
                    }

                    currentFinalRoster.Add(roundNumber, playersDrafted);
                }

                _finalRostersByYear.Add(year, currentFinalRoster);
            }

            _package.Save();
        }

        public void AddKeeperValues()
        {
            foreach (ExcelWorksheet tab in _package.Workbook.Worksheets)
            {
                Dictionary<int, List<string>> currentFinalRoster = new Dictionary<int, List<string>>();
                string year = tab.Name;

                for (int keeperCount = _keeperRangeStart.Item1; keeperCount <= _keeperRangeEnd.Item1; keeperCount++)
                {
                    for (int managerNumber = _keeperRangeStart.Item2; managerNumber <= _keeperRangeEnd.Item2; managerNumber++)
                    {
                        var value = tab.Cells[keeperCount, managerNumber].Value;
                        string playerName = (value == null ? "" : value.ToString());

                        if (!string.IsNullOrWhiteSpace(playerName))
                        {
                            if (playerName.Contains("("))
                            {
                                tab.Cells[keeperCount, managerNumber].Value = tab.Cells[keeperCount, managerNumber].Value.ToString().Split(" (")[0];
                                playerName = tab.Cells[keeperCount, managerNumber].Value.ToString();
                            }

                            Dictionary<string, PlayerModel> currentDraftBoard = _draftBoardsByYear[year];

                            if (currentDraftBoard.TryGetValue(playerName, out PlayerModel playerModel))
                            {
                                playerModel.IsKeeper = true;
                                playerModel.YearsKeptByCurrentManager++;

                                if (playerModel.RoundDrafted > 2)
                                {
                                    tab.Cells[keeperCount, managerNumber].Value += $" ({playerModel.RoundDrafted - 1})";
                                }
                                else
                                {
                                    tab.Cells[keeperCount, managerNumber].Value += $" (NA)";
                                }

                                currentDraftBoard[playerName] = playerModel;
                            }
                            else
                            {
                                tab.Cells[keeperCount, managerNumber].Value += $" (15)";
                            }
                        }
                    }
                }
            }

            _package.Save();
        }

        public void SubmitKeepers(Manager manager, string playerName, string year)
        {
            throw new NotImplementedException();
        }

        public void SubmitManagersDraft(Manager manager, List<string> playersDraftedInOrder, string year)
        {
            throw new NotImplementedException();
        }
    }
}
