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

namespace HlsView
{
    public partial class Settings : PhoneApplicationPage
    {
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;

        public Settings()
        {
            InitializeComponent();
        }

        private void chkHideScores_Checked(object sender, RoutedEventArgs e)
        {

        }

        void wc_DownloadStringCompletedHandler(object sender, DownloadStringCompletedEventArgs e)
        {
            JObject o = new JObject();
            try
            {
                string[] locations = e.Result.Split(new Char[] {','});
                foreach (string location in locations)
                {
                    string[] locData = location.Split(new Char[] { '"' });
                    locationPicker.Items.Add(locData[3]);
                }
                
            }
            catch (Exception)//System.Reflection.TargetInvocationException)
            {
                MessageBox.Show("Unable to load stream locatoins.");
            }
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            string hideScores = "";
            string gameType = "";
            string location = "";
            try
            {
                hideScores = (string)userSettings["HideScores"];
                gameType = (string)userSettings["GameType"];
                location = (string)userSettings["Location"];
            }
            catch
            {
                hideScores = "null";
                gameType = "null";
                location = "null";
            }

            if (location != "null")
            {
                locationPicker.SelectedItem = location;
            }
            else
            {
                locationPicker.SelectedItem = "North America - Central";
            }

            if (hideScores == "1")
            {
                chkHideScores.IsChecked = true;
            }

            switch (gameType)
            {
                case "FullGame":
                    rdoFullGames.IsChecked = true;
                    rdoHighlights.IsChecked = false;
                    rdoCondensed.IsChecked = false;
                    break;
                case "Highlights":
                    rdoFullGames.IsChecked = false;
                    rdoHighlights.IsChecked = true;
                    rdoCondensed.IsChecked = false;
                    break;
                case "Condensed":
                    rdoFullGames.IsChecked = false;
                    rdoHighlights.IsChecked = false;
                    rdoCondensed.IsChecked = true;
                    break;
                default:
                    rdoFullGames.IsChecked = false;
                    rdoHighlights.IsChecked = true;
                    rdoCondensed.IsChecked = false;
                    break;
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (chkHideScores.IsChecked == true)
            {
                try
                {
                    userSettings.Add("HideScores", "1");
                }
                catch (ArgumentException)
                {
                    userSettings["HideScores"] = "1";
                }

            }
            else
            {
                try
                {
                    userSettings.Add("HideScores", "0");
                }
                catch (ArgumentException)
                {
                    userSettings["HideScores"] = "0";
                }
            }
            if (rdoHighlights.IsChecked == true)
            {
                try
                {
                    userSettings.Add("GameType", "Highlights");
                }
                catch (ArgumentException)
                {
                    userSettings["GameType"] = "Highlights";
                }
            }
            else if (rdoFullGames.IsChecked == true)
            {
                try
                {
                    userSettings.Add("GameType", "FullGame");
                }
                catch (ArgumentException)
                {
                    userSettings["GameType"] = "FullGame";
                }
            }
            else if (rdoCondensed.IsChecked == true)
            {
                try
                {
                    userSettings.Add("GameType", "Condensed");
                }
                catch (ArgumentException)
                {
                    userSettings["GameType"] = "Condensed";
                }
            }

            try
            {
                userSettings.Add("Location", locationPicker.SelectedItem);
            }
            catch (ArgumentException)
            {
                userSettings["Location"] = locationPicker.SelectedItem;
            }

            NavigationService.GoBack();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            rdoFullGames.IsChecked = false;
            rdoCondensed.IsChecked = false;
        }

        private void rdoFullGames_Checked(object sender, RoutedEventArgs e)
        {
            rdoHighlights.IsChecked = false;
            rdoCondensed.IsChecked = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.GoBack();
        }

        private void rdoCondensed_Checked(object sender, RoutedEventArgs e)
        {
            rdoHighlights.IsChecked = false;
            rdoFullGames.IsChecked = false;
        }
    }
}