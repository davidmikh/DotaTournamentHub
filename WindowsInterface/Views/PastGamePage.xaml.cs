using System;
using WindowsInterface.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Template10.Services.NavigationService;

namespace WindowsInterface.Views
{
    public sealed partial class PastGamePage : Page
    {
        public PastGamePage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private void SpoilerResultBtnClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            //NavigationService.GetForFrame(Frame).Navigate(typeof(Views.TournamentPage), (long) btn.CommandParameter);
        }
    }
}
