using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.IsolatedStorage;
using System.Windows.Media;
using System.IO;
using System.Net.Http;

namespace HlsView
{
    public class Teams
    {
        public string TeamName
        {
            get;
            set;
        }
        public string League
        {
            get;
            set;
        }
        

        public Teams(string teamname, string league)
        {
            this.TeamName = teamname;
            this.League = league;
        }
    }
    public partial class PivotMain : PhoneApplicationPage
    {
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;
        List<string> goodDates = new List<string>();
        private ProgressIndicator _liveprogressIndicator = new ProgressIndicator(); //Create progress indicator to indicate system busy.
        private ProgressIndicator _ondemandprogressIndicator = new ProgressIndicator(); //Create progress indicator to indicate system busy.

        public PivotMain()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
            //progress indicator construct
            _liveprogressIndicator.Text = "Getting Info from HockeyStreams.com.";
            _ondemandprogressIndicator.Text = "Getting Info from HockeyStreams.com.";
            SystemTray.SetProgressIndicator(this, _liveprogressIndicator);
            SystemTray.SetProgressIndicator(this, _ondemandprogressIndicator);
            ShowLiveProgressIndicator();
            ShowOnDemandProgressIndicator();

            string authToken = (string)userSettings["Token"];
            string favteam = (string)userSettings["FavTeam"];
            btnTeam.Content = favteam;

            LoadTeams(authToken);
            GetOnDemandDates(authToken);

        }

        private async void GetOnDemandDates(string authToken)
        {
            HttpClient hc = new HttpClient();
            string link = "https://api.hockeystreams.com/GetOnDemandDates?token=" + authToken;
            string response = await hc.GetStringAsync(link);
            JObject o = new JObject();
            try
            {
                o = JObject.Parse(response);
                foreach (JToken date in o["dates"])
                {
                    goodDates.Add(date.ToString());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No Dates found for OnDemand Videos");
            }
            HideOnDemandProgressIndicator();
        }

        private async void LoadTeams(string authToken)
        {
            HttpClient hc = new HttpClient();
            string teamLink = "https://api.hockeystreams.com/ListTeams?token=" + authToken;
            string response = await hc.GetStringAsync(teamLink);
            JObject teams = new JObject();
            try
            {
                teams = JObject.Parse(response);
                List<Teams> source = new List<Teams>();
                foreach (JToken team in teams["teams"])
                {
                    source.Add(new Teams(team["name"].ToString(), team["league"].ToString()));
                }
                List<AlphaKeyGroup<Teams>> DataSource = AlphaKeyGroup<Teams>.CreateGroups(source,
                    System.Threading.Thread.CurrentThread.CurrentUICulture,
                    (Teams s) => { return s.TeamName; }, true);
                teamPicker.ItemsSource = DataSource;
            }
            catch (Exception)
            {
                MessageBox.Show("Teams could not be loaded from HockeyStreams.com");
            }

        }

        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appbarmenuitem = new ApplicationBarMenuItem("Settings...");
            ApplicationBarMenuItem appbarmenuitem2 = new ApplicationBarMenuItem("About");
            ApplicationBarMenuItem appbarmenuitem3 = new ApplicationBarMenuItem("Logout");
            ApplicationBar.MenuItems.Add(appbarmenuitem);
            ApplicationBar.MenuItems.Add(appbarmenuitem2);
            ApplicationBar.MenuItems.Add(appbarmenuitem3);
            appbarmenuitem.Click += appbarmenuitem_Click;
            appbarmenuitem2.Click += appbarmenuitem2_Click;
            appbarmenuitem3.Click += appbarmenuitem3_Click;
        }

        void appbarmenuitem3_Click(object sender, EventArgs e)
        {
            userSettings.Remove("Token");
            NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
        }
        void appbarmenuitem2_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        void appbarmenuitem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            
            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            //REMOVE LOGIN PAGE FROM MEMORY
            NavigationService.RemoveBackEntry();

        }

        private async void GetLiveGames(string link)
        {
            HttpClient hc = new HttpClient();
            string response = await hc.GetStringAsync(link);
            JObject o = new JObject();
            try
            {
                o = JObject.Parse(response);
            }
            catch (Exception)//System.Reflection.TargetInvocationException)
            {
                MessageBox.Show("No Live games found for this day.");
            }

            try
            {
                if (o["status"].ToString() == "Failed")
                {
                    MessageBox.Show("Please re-enter your credentials.");
                    NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
                }
            }
            catch (NullReferenceException)
            {

            }
            

            try
            {
                Button[] btnGames = new Button[o["schedule"].Count()];
                TextBlock[] txtInfo = new TextBlock[o["schedule"].Count()];
                int heightMargin = 0;
                int horizMargin = 0;
                int i = 0;
                ContentPanel.Height = 110 * o["schedule"].Count(); //135
                RemoveLiveContent();

                foreach (JToken game in o["schedule"])
                {
                    btnGames[i] = new Button { FontSize = 16, Tag = game["id"].ToString(), IsEnabled=false };
                    btnGames[i].VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    btnGames[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    btnGames[i].Content = new TextBlock { Text = game["awayTeam"].ToString() + " @ " + game["homeTeam"].ToString(), TextWrapping = TextWrapping.Wrap };
                    btnGames[i].Margin = new Thickness(horizMargin + 0, heightMargin - 0, 0, 0);
                    btnGames[i].Width = 450;
                    btnGames[i].Height = 100;
                    if (game["isPlaying"].ToString() == "1")
                    {
                        btnGames[i].IsEnabled = true;
                        btnGames[i].Click += GameList_Click;
                    }
                    
                    ContentPanel.Children.Add(btnGames[i]);

                    txtInfo[i] = new TextBlock { Text = "Start Time: " + game["startTime"].ToString() + " :: " + game["feedType"].ToString(),FontSize = 14 };
                    txtInfo[i].VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    txtInfo[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    txtInfo[i].Margin = new Thickness(horizMargin + 225, heightMargin + 90, 0, 0);
                    ContentPanel.Children.Add(txtInfo[i]);

                    heightMargin = heightMargin + 110;
                    i++;
                }
            }
            catch
            {
                HideLiveProgressIndicator();

            }
            HideLiveProgressIndicator();

        }

        public SolidColorBrush GetColorFromHexa(string hexaColor)
        {
            byte R = Convert.ToByte(hexaColor.Substring(1, 2), 16);
            byte G = Convert.ToByte(hexaColor.Substring(3, 2), 16);
            byte B = Convert.ToByte(hexaColor.Substring(5, 2), 16);
            SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(0xFF, R, G, B));
            return scb;
        }

        void GameList_Click(object sender, RoutedEventArgs e)
        {
            Button target = sender as Button;
            NavigationService.Navigate(new Uri("/LiveStreamDetail.xaml?streamID=" + target.Tag.ToString(), UriKind.Relative));
        }

        void GameList_Click_OnDemand(object sender, RoutedEventArgs e)
        {
            Button target = sender as Button;
            NavigationService.Navigate(new Uri("/OnDemandDetail.xaml?streamID=" + target.Tag.ToString(), UriKind.Relative));
        }

        void ShowLiveProgressIndicator()
        {
            _liveprogressIndicator.IsVisible = true;
            _liveprogressIndicator.IsIndeterminate = true;
        }

        void HideLiveProgressIndicator()
        {
            _liveprogressIndicator.IsVisible = false;
            _liveprogressIndicator.IsIndeterminate = false;
        }
        void ShowOnDemandProgressIndicator()
        {
            _ondemandprogressIndicator.IsVisible = true;
            _ondemandprogressIndicator.IsIndeterminate = true;
        }

        void HideOnDemandProgressIndicator()
        {
            _ondemandprogressIndicator.IsVisible = false;
            _ondemandprogressIndicator.IsIndeterminate = false;
        }

        private void RemoveLiveContent()
        {
            int contentLength = ContentPanel.Children.Count;
            for (int child = 0; child < contentLength; child++)
            {
                ContentPanel.Children.RemoveAt(0);

            }
        }

        private void liveDate_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            ShowLiveProgressIndicator();
            RemoveLiveContent();
            
            //GET TOKEN FROM MEMORY
            string authToken = (string)userSettings["Token"];

            //GET TODAYS LIVE GAMES
            GetLiveGames("https://api.hockeystreams.com/GetLive?date=" + liveDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&token=" + authToken);
        }

        private void ondemandDate_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            ShowOnDemandProgressIndicator();
            //GET TOKEN FROM MEMORY
            string authToken = (string)userSettings["Token"];
            string favteam = (string)userSettings["FavTeam"];

            RemoveContent();

            //GET TODAYS LIVE GAMES
            if (goodDates.Contains(ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy")))
            {
                if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == true))
                {
                   GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&team=" + btnTeam.Content + "&token=" + authToken);
                }
                else if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == false))
                {
                    GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&token=" + authToken);
                }
                else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == true))
                {
                    GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?team=" + btnTeam.Content + "&token=" + authToken);
                }
                else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == false))
                {
                    GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?&token=" + authToken);
                }
            }
            else
            {
                MessageBox.Show("There are no OnDemand Videos for that day.");
            }

        }

        private void pvtLivePivot_Loaded(object sender, RoutedEventArgs e)
        {
            ShowLiveProgressIndicator();

            if (liveDate.Value.Value.Date.ToShortDateString() == DateTime.Now.Date.ToShortDateString())
            {
                //GET TOKEN FROM MEMORY
                string authToken = (string)userSettings["Token"];

                //GET TODAYS LIVE GAMES
                GetLiveGames("https://api.hockeystreams.com/GetLive?token=" + authToken);
            }
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ODContentPanel_Loaded(object sender, RoutedEventArgs e)
        {
            ShowOnDemandProgressIndicator();
            //GET TOKEN FROM MEMORY
            string authToken = (string)userSettings["Token"];
            string favteam = (string)userSettings["FavTeam"];

            //GET TODAYS LIVE GAMES
            if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == true))
            {
                if (goodDates.Contains(ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy")))
                {
                    GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&team=" + btnTeam.Content + "&token=" + authToken);
                }
            }
            else if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == false))
            {
                if (goodDates.Contains(ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy")))
                {
                    GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&token=" + authToken);
                }
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == true))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?team=" + btnTeam.Content + "&token=" + authToken);
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == false))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?&token=" + authToken);
            }
        }
        private async void GetOnDemandGames(string link)
        {
            HttpClient hc = new HttpClient();
            string response = await hc.GetStringAsync(link);
            JObject o = new JObject();
            try
            {
                o = JObject.Parse(response);
            }
            catch (Exception)//System.Reflection.TargetInvocationException)
            {
                //MessageBox.Show("No OnDemand games found matching these criteria.");
                HideOnDemandProgressIndicator();
            }

            try
            {
                Button[] btnGames = new Button[o["ondemand"].Count()];
                TextBlock[] txtInfo = new TextBlock[o["ondemand"].Count()];
                int heightMargin = 0;
                int horizMargin = 0;
                int i = 0;
                ODContentPanel.Height = 110 * o["ondemand"].Count(); //135
                RemoveContent();

                foreach (JToken game in o["ondemand"])
                {
                    btnGames[i] = new Button { FontSize = 16, Tag = game["id"].ToString() };
                    btnGames[i].VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    btnGames[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    btnGames[i].Content = new TextBlock { Text = game["awayTeam"].ToString() + " @ " + game["homeTeam"].ToString(), TextWrapping = TextWrapping.Wrap };
                    btnGames[i].Margin = new Thickness(horizMargin + 0, heightMargin - 0, 0, 0);
                    btnGames[i].Click += GameList_Click_OnDemand;
                    btnGames[i].Width = 450;
                    btnGames[i].Height = 100;

                    txtInfo[i] = new TextBlock { Text = "Original Air Date: " + game["date"].ToString()+" :: " + game["feedType"].ToString(), FontSize = 14 };
                    txtInfo[i].VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    txtInfo[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    txtInfo[i].Margin = new Thickness(horizMargin + 185, heightMargin + 90, 0, 0);
                    
                    ODContentPanel.Children.Add(btnGames[i]);
                    ODContentPanel.Children.Add(txtInfo[i]);

                    heightMargin = heightMargin + 110;
                    i++;
                }
            }
            catch
            {
                HideOnDemandProgressIndicator();
            }
            HideOnDemandProgressIndicator();
        }

        private void btnTeam_Click(object sender, RoutedEventArgs e)
        {
            chkTeam.IsEnabled = false;
            teamPicker.Visibility = System.Windows.Visibility.Visible;
            ODContentPanel.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void teamPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            chkTeam.IsEnabled = true;
            ShowOnDemandProgressIndicator();
            Teams s = teamPicker.SelectedItem as Teams;
            btnTeam.Content = s.TeamName;
            teamPicker.Visibility = System.Windows.Visibility.Collapsed;
            ODContentPanel.Visibility = System.Windows.Visibility.Visible;
            //GET TOKEN FROM MEMORY
            string authToken = (string)userSettings["Token"];
            string favteam = (string)userSettings["FavTeam"];

            RemoveContent();
            
            //GET TODAYS LIVE GAMES
            if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == true))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&team=" + btnTeam.Content + "&token=" + authToken);
            }
            else if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == false))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&token=" + authToken);
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == true))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?team=" + btnTeam.Content + "&token=" + authToken);
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == false))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?&token=" + authToken);
            }
            

        }

        private void RemoveContent()
        {
            int contentLength = ODContentPanel.Children.Count;
            for (int child = 0; child < contentLength; child++)
            {
                ODContentPanel.Children.RemoveAt(0);

            }
        }

        private void chkDate_Checked(object sender, RoutedEventArgs e)
        {
            ondemandDate.IsEnabled = true;
            ShowOnDemandProgressIndicator();
            string authToken = (string)userSettings["Token"];
            string favteam = (string)userSettings["FavTeam"];

            RemoveContent();

            //GET TODAYS LIVE GAMES
            if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == true))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&team=" + btnTeam.Content + "&token=" + authToken);
            }
            else if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == false))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&token=" + authToken);
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == true))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?team=" + btnTeam.Content + "&token=" + authToken);
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == false))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?&token=" + authToken);
            }
        }


        private void chkDate_Unchecked(object sender, RoutedEventArgs e)
        {
            ShowOnDemandProgressIndicator();
            ondemandDate.IsEnabled = false;
            string authToken = (string)userSettings["Token"];
            string favteam = (string)userSettings["FavTeam"];

            RemoveContent();

            //GET TODAYS LIVE GAMES
            if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == true))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&team=" + btnTeam.Content + "&token=" + authToken);
            }
            else if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == false))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&token=" + authToken);
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == true))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?team=" + btnTeam.Content + "&token=" + authToken);
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == false))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?&token=" + authToken);
            }
        }

        private void pvtOnDemand_Loaded(object sender, RoutedEventArgs e)
        {
            if (chkDate.IsChecked == true)
            {
                ondemandDate.IsEnabled = true;
            }
            else
            {
                ondemandDate.IsEnabled = false;
            }

            if (chkTeam.IsChecked == true)
            {
                btnTeam.IsEnabled = true;
            }
            else
            {
                btnTeam.IsEnabled = false;
            }

        }

        private void chkTeam_Checked(object sender, RoutedEventArgs e)
        {
           ShowOnDemandProgressIndicator();
           btnTeam.IsEnabled = true;
           string authToken = (string)userSettings["Token"];
           string favteam = (string)userSettings["FavTeam"];

           RemoveContent();

           //GET TODAYS LIVE GAMES
            if ((chkDate.IsChecked==true) && (chkTeam.IsChecked==true))
            {
               GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&team=" + btnTeam.Content + "&token=" + authToken);
            }
            else if ((chkDate.IsChecked==true) && (chkTeam.IsChecked==false))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy")+"&token=" + authToken);
            }
            else if ((chkDate.IsChecked==false)&&(chkTeam.IsChecked==true))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?team=" + btnTeam.Content + "&token=" + authToken);
            }
            else if ((chkDate.IsChecked == false)&& (chkTeam.IsChecked ==false))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?&token=" + authToken);
            }
        }

        private void chkTeam_Unchecked(object sender, RoutedEventArgs e)
        {
            ShowOnDemandProgressIndicator();
            btnTeam.IsEnabled = false;
            string authToken = (string)userSettings["Token"];
            string favteam = (string)userSettings["FavTeam"];

            RemoveContent();

            //GET TODAYS LIVE GAMES
            if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == true))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&team=" + btnTeam.Content + "&token=" + authToken);
            }
            else if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == false))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&token=" + authToken);
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == true))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?team=" + btnTeam.Content + "&token=" + authToken);
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == false))
            {
                GetOnDemandGames("https://api.hockeystreams.com/GetOnDemand?&token=" + authToken);
            }
        }
    }
}