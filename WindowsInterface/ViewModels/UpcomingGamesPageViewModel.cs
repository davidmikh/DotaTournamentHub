using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using WindowsInterface.Models;
using CoreDataModels;

namespace WindowsInterface.ViewModels
{
    public class UpcomingGamesPageViewModel : ViewModelBase
    {
        public UpcomingGamesPageViewModel()
        {
            //Used to easily display information in the xaml editor without launching the program
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Tournaments = new List<TournamentModel>
                    {
                    new TournamentModel("The International 6",
                        new List<SeriesModel>
                        {
                            new SeriesModel(new Team("EG", null, null, null, null),
                            new Team("Na'Vi", null, null, null, null),
                            new DateTime(2016, 9, 10))
                        },
                        new DateTime(2016, 8, 18))
                    };
                //Value = "Designtime value";
            }
        }

        //List<TournamentModel> _Tournaments = new List<TournamentModel>();
        List<TournamentModel> _Tournaments = new List<TournamentModel>
                    {
                    new TournamentModel("The International 6",
                        new List<SeriesModel>
                        {
                            new SeriesModel(new Team("EG", null, null, null, null),
                            new Team("Na'Vi", null, null, null, null),
                            new DateTime(2016, 9, 10))
                        },
                        new DateTime(2016, 8, 18))
                    };
        public List<TournamentModel> Tournaments { get { return _Tournaments; } set { Set(ref _Tournaments, value); } }

        //string _Value = "Gas";
        //public string Value { get { return _Value; } set { Set(ref _Value, value); } }

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

