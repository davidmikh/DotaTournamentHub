using CoreDataModels;
using DataManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using WindowsInterface.Models;

namespace WindowsInterface.ViewModels
{
    public class TournamentPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private long id;
        private TournamentManager tournamentManager;
        private TournamentModel _Tournament;
        public TournamentModel Tournament { get { return _Tournament; } set { Set(ref _Tournament, value); } }
        private ObservableCollection<MatchModel> _LiveMatches;
        public ObservableCollection<MatchModel> LiveMatches { get { return _LiveMatches; } set { Set(ref _LiveMatches, value); } }

        public TournamentPageViewModel()
        {
            //Used to easily display information in the xaml editor without launching the program
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Tournament = new TournamentModel()
                {
                    ID = 4664,
                    Name = "The International 2016",
                    Matches = new ObservableCollection<MatchModel>
                    {
                        new MatchModel
                        {
                            ID = 2558534849,
                            Radiant = new TeamModel
                            {
                                ID = 2163,
                                LogoURL = new Uri("http://riki.dotabuff.net/t/l/XNAr01Hbpm.png"),
                                Name = "Team Liquid"
                            },
                            Dire = new TeamModel
                            {
                                ID = 36,
                                LogoURL = new Uri("http://riki.dotabuff.net/t/l/3QxyeKbMK6.png"),
                                Name = "Natus Vincere"
                            },
                            IsLive = true
                        },
                        new MatchModel
                        {
                            ID = 2551170074,
                            Radiant = new TeamModel
                            {
                                ID = 2586976,
                                LogoURL = new Uri("http://riki.dotabuff.net/t/l/JzKKxkMt36.png"),
                                Name = "OG"
                            },
                            Dire = new TeamModel
                            {
                                ID = 36,
                                LogoURL = new Uri("http://riki.dotabuff.net/t/l/3QxyeKbMK6.png"),
                                Name = "Natus Vincere"
                            },
                            IsLive = false
                        },
                        new MatchModel
                        {
                            ID = 2551023876,
                            Radiant = new TeamModel
                            {
                                ID = 1836806,
                                LogoURL = new Uri("http://riki.dotabuff.net/t/l/Q3h09RN8Of.png"),
                                Name = "Wings Gaming"
                            },
                            Dire = new TeamModel
                            {
                                ID = 36,
                                LogoURL = new Uri("http://riki.dotabuff.net/t/l/3QxyeKbMK6.png"),
                                Name = "Natus Vincere"
                            },
                            IsLive = false
                        },
                    }
                };
            }
            else
            {
                tournamentManager = new TournamentManager();
                //TODO: Enable some sort of loading gif
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            id = (suspensionState.ContainsKey(nameof(id))) ? (long) suspensionState[nameof(id)] : (long)parameter;
            //TODO: Should be a way to optimize this so it isn't so slow to get tournament matches. Might need to do on manager level
            //TODO: This can be improved by one of two ways, could potentially do both:
            //1) Make this function, GetMatchesForTournament, asyncrhonous and give it a delegate as a parameter. The delegate will refer to
            //a function in this class that will update the value of Tournament.Matches so that it can be done after page load
            //2) Replace GetMatchesForTournament with another function that doesn't need to call GetMatchDetails for each match in GetMatchHistory. Use the info we get instead from
            //GetMatchHistory to make some basic display somehow, won't be as good as the LiveGames one but at least it will be way faster. Keep the old function in case you ever need it.
            var pastMatches = tournamentManager.GetMatchesForTournament(id);
            var liveJsonMatches = tournamentManager.GetLiveTournamentGames().Where(t => t.Tournament.ID == id);
            ObservableCollection<MatchModel> matches = new ObservableCollection<MatchModel>();
            ObservableCollection<MatchModel> liveMatches = new ObservableCollection<MatchModel>();
            foreach (var match in liveJsonMatches)
            {
                liveMatches.Add(convertMatchToMatchModel(match, true));
            }
            foreach (var match in pastMatches)
            {
                matches.Add(convertMatchToMatchModel(match, false));
            }
            LiveMatches = liveMatches;
            Tournament = new TournamentModel()
            {
                ID = id,
                Name = liveJsonMatches.First().Tournament.Name,
                Matches = matches,
            };
            //TODO: Disable whatever the loading gif you create
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {
                suspensionState[nameof(id)] = id;
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }

        private MatchModel convertMatchToMatchModel(Match match, bool isLive)
        {
            return new MatchModel
            {
                ID = match.ID,
                Radiant = new TeamModel
                {
                    ID = match.Radiant.OfficialTeam.ID,
                    Name = match.Radiant.OfficialTeam.Name,
                    LogoURL = match.Radiant.OfficialTeam.LogoURL
                },
                Dire = new TeamModel
                {
                    ID = match.Dire.OfficialTeam.ID,
                    Name = match.Dire.OfficialTeam.Name,
                    LogoURL = match.Dire.OfficialTeam.LogoURL
                },
                IsLive = isLive,
            };
        }
    }
}

