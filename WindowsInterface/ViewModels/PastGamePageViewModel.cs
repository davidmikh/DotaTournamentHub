using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using WindowsInterface.Models;
using CoreDataModels;
using DataManager;
using System.Collections.ObjectModel;

namespace WindowsInterface.ViewModels
{
    public class PastGamePageViewModel : ViewModelBase
    {
        private TournamentManager tournamentManager;
        private PastMatchInfoModel pastMatch;
        public PastMatchInfoModel PastMatch { get { return pastMatch; } set { Set(ref pastMatch, value); } }

        public PastGamePageViewModel()
        {
            //Used to easily display information in the xaml editor without launching the program
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                List<GamePlayerModel> players = new List<GamePlayerModel>();
                for(int i = 0; i < 10; i++)
                {
                    players.Add(new GamePlayerModel()
                    {
                        Assists = 3,
                        Deaths = 4,
                        Denies = 2,
                        GPM = 222,
                        Hero = "Meepo",
                        Items = new string[6] { "Tango", "Tango", "Tango", "Tango", "Tango", "Tango" },
                        Kills = 3,
                        LastHits = 60,
                        Level = 25,
                        NetWorth = 19000,
                        Slot = i,
                        XPM = 715,
                        Player = new OfficialPlayerModel()
                        {
                            CountryCode = "1",
                            ID = 12,
                            Image = new Uri(""),
                            Name = "Dondo",
                            RealName = "Danil",
                            URL = new Uri(""),
                        }
                    });
                }

                pastMatch = new PastMatchInfoModel()
                {
                    ID = 123,
                    Tournament = new TournamentModel()
                    {
                        ID = 333333322,
                        Name = "TestTourny",
                        Matches = null,
                    },
                    StartTime = new DateTime(0),
                    Duration = new TimeSpan(100000),
                    GameType = 1,
                    RadiantVictory = false,
                    Radiant = new GameTeamModel()
                    {
                        Bans = new string[5] { "Monkey King", "Doom", "Viper", "Visage", "Axe" },
                        BarracksState = 1,
                        Kills = 22,
                        OfficialTeam = new OfficialTeamModel()
                        {
                            ID = 1,
                            LogoURL = new Uri("https://riki.dotabuff.com/t/l/11PExo6a83w.png"),
                            Name = "LULQUID",
                        },
                        TowerState = 1,
                        Players = players.Take(5),
                    },
                    Dire = new GameTeamModel()
                    {
                        Bans = new string[5] { "Alchemist", "Magnus", "Enigma", "Puck", "Pudge" },
                        BarracksState = 1,
                        Kills = 22,
                        OfficialTeam = new OfficialTeamModel()
                        {
                            ID = 1,
                            LogoURL = new Uri("https://riki.dotabuff.com/t/l/11PExo6a83w.png"),
                            Name = "NAR'VI",
                        },
                        TowerState = 1,
                        Players = players.Skip(5),
                    }
                };
            }
            else
            {
                tournamentManager = new TournamentManager();
                pastMatch = new PastMatchInfoModel();

                /*
                var liveGames = tournamentManager.GetLiveTournamentGames();
                var tournaments = liveGames.GroupBy(t => t.Tournament.ID);

                foreach (var tournament in tournaments)
                {
                    var matches = new ObservableCollection<MatchModel>();
                    foreach (var match in tournament)
                    {
                        matches.Add(new MatchModel
                        {
                            ID = match.ID,
                            Radiant = new OfficialTeamModel
                            {
                                ID = match.Radiant.OfficialTeam.ID,
                                LogoURL = match.Radiant.OfficialTeam.LogoURL,
                                Name = match.Radiant.OfficialTeam.Name,
                            },
                            Dire = new OfficialTeamModel
                            {
                                ID = match.Dire.OfficialTeam.ID,
                                LogoURL = match.Dire.OfficialTeam.LogoURL,
                                Name = match.Dire.OfficialTeam.Name,
                            }
                        });
                    }
                    _Tournaments.Add(new TournamentModel
                    {
                        ID = tournament.Key,
                        Name = tournament.Select(t => t.Tournament.Name).First(),
                        Matches = matches
                    });
                }
                */
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.Any())
            {
                //Value = suspensionState[nameof(Value)]?.ToString();
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {
                //suspensionState[nameof(Value)] = Value;
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }

        //public void GotoDetailsPage() =>
        //NavigationService.Navigate(typeof(Views.DetailPage), Value);

        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);

    }
}

