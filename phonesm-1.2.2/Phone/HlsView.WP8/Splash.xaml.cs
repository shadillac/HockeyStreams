using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace HlsView
{
    public partial class Splash : PhoneApplicationPage
    {
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;
        public Splash()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
            try
            {
                string authToken = (string)userSettings["Token"];
                NavigationService.Navigate(new Uri("/PivotMain.xaml",UriKind.Relative));
            }
            catch (KeyNotFoundException)
            {
                NavigationService.Navigate(new Uri("/Login.xaml",UriKind.Relative));
            }
        }
    }
}