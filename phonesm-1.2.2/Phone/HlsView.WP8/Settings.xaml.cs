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
                    locationPicker.SelectedItem = (string)userSettings["Location"];
                }
                catch (Exception)
                {
                    locationPicker.SelectedItem = "North America - Central";
                }
            }
            string hideScores = "";
            string gameType = "";
            try
            {
                hideScores = (string)userSettings["HideScores"];
                gameType = (string)userSettings["GameType"];
            }
            catch
            {
                hideScores = "null";
                gameType = "null";

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

        private void btnIP_Click(object sender, RoutedEventArgs e)
        {
            WebClient wClient = new WebClient();
            wClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            wClient.UploadStringCompleted += IPUploadCompleted;
            string parameters2 = "token=" + (string)userSettings["Token"];
            wClient.UploadStringAsync(new Uri("https://api.hockeystreams.com/IPException?"), "POST", parameters2);
        }

        void IPUploadCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            JObject o = new JObject();
            try
            {
                o = JObject.Parse(e.Result);
                if (o["status"].ToString()=="Success")
                {
                    MessageBox.Show("IP Exception Created Successfully.");
                }
                else
                {
                    MessageBox.Show("Error in creating IP Exception");
                }
            }
            catch (System.Reflection.TargetInvocationException)
            {
                //MessageBox.Show("Username or Password Incorrect.  Please verify and reenter.");
            }
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