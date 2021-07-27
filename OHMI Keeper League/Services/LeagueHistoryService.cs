using Microsoft.EntityFrameworkCore;
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

namespace OHMI_Keeper_League.Services
{
    public class LeagueHistoryService : ILeagueHistoryService
    {
        private readonly OHMIKeeperLeagueContext _dbContext;
        private IHttpClientWrapper _client;
        private static ExcelPackage _package;
        private string _token;
        private Dictionary<string, Dictionary<string, PlayerModel>> _draftBoardsByYear;
        private Dictionary<string, Dictionary<int, List<string>>> _finalRostersByYear;
        private static Tuple<int, int> _keeperRangeStart = new Tuple<int, int>(19, 1);
        private static Tuple<int, int> _keeperRangeEnd = new Tuple<int, int>(21, 12);

        public LeagueHistoryService(IHttpClientWrapper client, OHMIKeeperLeagueContext dbContext)
        {
            _dbContext = dbContext;
            _client = client;
            Setup();
        }

        void Setup()
        {
            FileInfo file = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.GetDirectories("Reports").FirstOrDefault().GetFiles(Configurations.FileName).FirstOrDefault();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _package = new ExcelPackage(file);

            //GetDraftBoards();
            //GetFinalRosters();
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

                            currentDraftBoard.Add(playerName.Replace("'", "").ToUpper(), playerModel);
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

                        playersDrafted.Add(playerName.Replace("'", "").ToUpper());

                        if (!string.IsNullOrWhiteSpace(playerName))
                        {
                            if (playerName.Contains("("))
                            {
                                tab.Cells[roundNumber, managerNumber].Value = tab.Cells[roundNumber, managerNumber].Value.ToString().Split(" (")[0];
                                playerName = tab.Cells[roundNumber, managerNumber].Value.ToString();
                            }

                            Dictionary<string, PlayerModel> currentDraftBoard = _draftBoardsByYear[year];

                            if (currentDraftBoard.TryGetValue(playerName.Replace("'", "").ToUpper(), out PlayerModel playerModel))
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

        public async Task AddKeeperValues()
        {
            //_token = await _client.RefreshAuthorization();

            foreach (ExcelWorksheet tab in _package.Workbook.Worksheets)
            {
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

        void AddManagersToDB()
        {
            List<Manager> managers = new List<Manager>()
            {
                new Manager
                {
                    FullName = "Michael Doney",
                    EmailAddress = "michaeldoney@comcast.net",
                    StartDate = new DateTime(2019, 8, 13)
                },
                new Manager
                {
                    FullName = "Tom Doney",
                    EmailAddress = "tomdoney@gmail.com",
                    StartDate = new DateTime(2019, 8, 13)
                },
                new Manager
                {
                    FullName = "Gail Doney",
                    EmailAddress = "gaildoney@gmail.com",
                    StartDate = new DateTime(2019, 8, 13)
                },
                new Manager
                {
                    FullName = "Lunden Carpenter",
                    EmailAddress = "lundcarp@umich.edu",
                    StartDate = new DateTime(2019, 8, 13)
                },
                new Manager
                {
                    FullName = "Roger Samson",
                    EmailAddress = "rogerpsamson@gmail.com",
                    StartDate = new DateTime(2019, 8, 13)
                },
                new Manager
                {
                    FullName = "Jenny Jacob",
                    EmailAddress = "jenny.jacob@hansoninc.com",
                    StartDate = new DateTime(2019, 8, 13)
                },
                new Manager
                {
                    FullName = "George Jacob",
                    EmailAddress = "345george345@gmail.com",
                    StartDate = new DateTime(2019, 8, 13)
                },
                new Manager
                {
                    FullName = "Torin Carpenter",
                    EmailAddress = "carpentert15@gmail.com",
                    StartDate = new DateTime(2019, 8, 13)
                },
                new Manager
                {
                    FullName = "Bobby Willen",
                    EmailAddress = "willenr@mail.gvsu.com",
                    StartDate = new DateTime(2019, 8, 13)
                },
                new Manager
                {
                    FullName = "Thomas Marine",
                    EmailAddress = "thomas.marine42@gmail.com",
                    StartDate = new DateTime(2019, 8, 13)
                },
                new Manager
                {
                    FullName = "Andrew Giza",
                    EmailAddress = "acgiza@aol.com",
                    StartDate = new DateTime(2019, 8, 13)
                },
                new Manager
                {
                    FullName = "Brien Garvey",
                    EmailAddress = "brien.garvey@gmail.com",
                    StartDate = new DateTime(2019, 8, 13)
                },
            };

            _dbContext.Managers.AddRange(managers);
            _dbContext.SaveChanges();
        }

        public void SubmitKeepers(Manager manager, string playerName, string year)
        {
            
        }

        public void SubmitManagersDraft(Manager manager, List<string> playersDraftedInOrder, string year)
        {
            throw new NotImplementedException();
        }
    }
}
