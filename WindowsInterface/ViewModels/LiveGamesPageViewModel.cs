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

namespace WindowsInterface.ViewModels
{
    public class LiveGamesPageViewModel : ViewModelBase
    {
        private TournamentManager tournamentManager;
        private List<TournamentModel> _Tournaments;
        public List<TournamentModel> Tournaments { get { return _Tournaments; } set { Set(ref _Tournaments, value); } }

        public LiveGamesPageViewModel()
        {
            //Used to easily display information in the xaml editor without launching the program
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Tournaments = new List<TournamentModel>
                {
                    new TournamentModel
                    {
                        ID = 4664,
                        Name = "The International 2016",
                        Matches = new List<MatchModel>
                        {
                            new MatchModel
                            {
                                ID = 2558534849,
                                Radiant = new TeamModel
                                {
                                    ID = 2163,
                                    Name = "Team Liquid",
                                    LogoURL = new Uri("http://riki.dotabuff.net/t/l/XNAr01Hbpm.png")
                                },
                                Dire = new TeamModel
                                {
                                    ID = 36,
                                    Name = "Natus Vincere",
                                    LogoURL = new Uri("http://riki.dotabuff.net/t/l/3QxyeKbMK6.png")
                                },
                            },
                            new MatchModel
                            {
                                ID = 2551474091,
                                Radiant = new TeamModel
                                {
                                    ID = 2512249,
                                    Name = "Digital Chaos",
                                    LogoURL = new Uri("http://riki.dotabuff.net/t/l/pAwIGd6wVT.png")
                                },
                                Dire = new TeamModel
                                {
                                    ID = 2163,
                                    Name = "Team Liquid",
                                    LogoURL = new Uri("http://riki.dotabuff.net/t/l/XNAr01Hbpm.png")
                                },
                            }
                        }
                    },
                    new TournamentModel
                    {
                        ID = 3781,
                        Name = "The Summit 4",
                        Matches = new List<MatchModel>
                        {
                            new MatchModel
                            {
                                ID = 1995989266,
                                Radiant = new TeamModel
                                {
                                    ID = 39,
                                    Name = "Evil Geniuses",
                                    LogoURL = new Uri("http://riki.dotabuff.net/t/l/aI2hxnL46H.png")
                                },
                                Dire = new TeamModel
                                {
                                    ID = 726228,
                                    Name = "Vici Gaming",
                                    LogoURL = new Uri("http://riki.dotabuff.net/t/l/2bR6gRR8zG.png")
                                },
                            }
                        }
                    }
                };
            }
            else
            {
                tournamentManager = new TournamentManager();
                _Tournaments = new List<TournamentModel>();

                var liveGames = tournamentManager.GetLiveTournamentGames();
                var tournaments = liveGames.GroupBy(t => t.Tournament.ID);

                foreach (var tournament in tournaments)
                {
                    var matches = new List<MatchModel>();
                    foreach (var match in tournament)
                    {
                        matches.Add(new MatchModel
                        {
                            ID = match.ID,
                            Radiant = new TeamModel
                            {
                                ID = match.Radiant.OfficialTeam.ID,
                                LogoURL = match.Radiant.OfficialTeam.LogoURL,
                                Name = match.Radiant.OfficialTeam.Name,
                            },
                            Dire = new TeamModel
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
                        Matches = matches,
                    });
                }
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

