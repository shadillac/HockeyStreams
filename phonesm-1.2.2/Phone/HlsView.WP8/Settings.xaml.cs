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
using Newtonsoft.Json.Linq;
using Microsoft.Phone.Tasks;

namespace HlsView
{
    public partial class Settings : PhoneApplicationPage
    {
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;

        public Settings()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (locationPicker.Items.Count == 0)
            {
                try
                {
                    locationPicker.Items.Add("Asia");
                    locationPicker.Items.Add("Europe");
                    locationPicker.Items.Add("North America - Central");
                    locationPicker.Items.Add("North America - East");
                    locationPicker.Items.Add("North America - East Canada");
                    locationPicker.Items.Add("North America - West");
                    locationPicker.SelectedItem = (string)userSettings["Location"];
                }
                catch (Exception)
                {
                    locationPicker.SelectedItem = "North America - Central";
                }
            }

            try
            {
                string scores = (string)userSettings["HideScores"];
                if (scores == "1")
                {
                    chkHideScores.IsChecked = true;
                }
                else
                {
                    chkHideScores.IsChecked = false;
                }
            }
            catch
            {
                chkHideScores.IsChecked = false;
            }
           

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                userSettings.Add("Location", locationPicker.SelectedItem);
            }
            catch (ArgumentException)
            {
                userSettings["Location"] = locationPicker.SelectedItem;
            }

            try
            {
                if (chkHideScores.IsChecked == true)
                {
                    userSettings.Add("HideScores", "1");
                }
                else
                {
                    userSettings.Add("HideScores", "0");
                }
            }
            catch (Exception)
            {
                if (chkHideScores.IsChecked == true)
                {
                    userSettings["HideScores"] = "1";
                }
                else
                {
                    userSettings["HideScores"] = "0";
                }
            }

            NavigationService.GoBack();
        }

        
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.GoBack();
        }

        private void locationPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void locationPicker_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnIP_Click_1(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.hockeystreams.com/devices");
            webBrowserTask.Show();
        }


        
    }
}