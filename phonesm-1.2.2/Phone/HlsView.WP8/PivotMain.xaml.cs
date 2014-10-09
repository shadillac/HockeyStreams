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
        private ProgressIndicator _progressIndicator = new ProgressIndicator(); //Create progress indicator to indicate system busy.

        public PivotMain()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
            //progress indicator construct
            _progressIndicator.IsVisible = false;
            _progressIndicator.IsIndeterminate = true;
            _progressIndicator.Text = "Getting Info from HockeyStreams.com.";
            SystemTray.SetProgressIndicator(this, _progressIndicator);
            ShowProgressIndicator();

            string authToken = (string)userSettings["Token"];
            string favteam = (string)userSettings["FavTeam"];
            btnTeam.Content = favteam;
            List<Teams> source = new List<Teams>();
            source.Add(new Teams("Abbotsford Heat", "AHL"));
            source.Add(new Teams("Adirondack Phantoms", "AHL"));
            source.Add(new Teams("Albany Devils", "AHL"));
            source.Add(new Teams("Binghamton Senators", "AHL"));
            source.Add(new Teams("Bridgeport Sound Tigers", "AHL"));
            source.Add(new Teams("Charlotte Checkers", "AHL"));
            source.Add(new Teams("Chicago Wolves", "AHL"));
            source.Add(new Teams("Farjestad BK", "AHL"));
            source.Add(new Teams("Grand Rapids Griffins", "AHL"));
            source.Add(new Teams("Hamilton Bulldogs", "AHL"));
            source.Add(new Teams("Hartford Wolf Pack ", "AHL"));
            source.Add(new Teams("Hershey Bears", "AHL"));
            source.Add(new Teams("Iowa Wild", "AHL"));
            source.Add(new Teams("Lake Erie Monsters", "AHL"));
            source.Add(new Teams("Manchester Monarchs", "AHL"));
            source.Add(new Teams("Milwaukee Admirals", "AHL"));
            source.Add(new Teams("Norfolk Admirals", "AHL"));
            source.Add(new Teams("Oklahoma City Barons", "AHL"));
            source.Add(new Teams("Portland Pirates", "AHL"));
            source.Add(new Teams("Providence Bruins", "AHL"));
            source.Add(new Teams("Rochester Americans", "AHL"));
            source.Add(new Teams("Rockford IceHogs", "AHL"));
            source.Add(new Teams("San Antonio Rampage", "AHL"));
            source.Add(new Teams("Springfield Falcons", "AHL"));
            source.Add(new Teams("St Johns IceCaps", "AHL"));
            source.Add(new Teams("Syracuse Crunch", "AHL"));
            source.Add(new Teams("Texas Stars", "AHL"));
            source.Add(new Teams("Toronto Marlies", "AHL"));
            source.Add(new Teams("Utica Comets", "AHL"));
            source.Add(new Teams("Wilkes-Barre Scranton Penguins", "AHL"));
            source.Add(new Teams("Worcester Sharks", "AHL"));
            source.Add(new Teams("Belarus", "IIHF"));
            source.Add(new Teams("Czech Republic", "IIHF"));
            source.Add(new Teams("Finland", "IIHF"));
            source.Add(new Teams("France", "IIHF"));
            source.Add(new Teams("Italy", "IIHF"));
            source.Add(new Teams("Kazakhstan", "IIHF"));
            source.Add(new Teams("Norway", "IIHF"));
            source.Add(new Teams("Russia", "IIHF"));
            source.Add(new Teams("Slovakia", "IIHF"));
            source.Add(new Teams("Sweden", "IIHF"));
            source.Add(new Teams("Switzerland", "IIHF"));
            source.Add(new Teams("Anaheim Ducks", "NHL"));
            source.Add(new Teams("Boston Bruins", "NHL"));
            source.Add(new Teams("Buffalo Sabres", "NHL"));
            source.Add(new Teams("Calgary Flames", "NHL"));
            source.Add(new Teams("Carolina Hurricanes", "NHL"));
            source.Add(new Teams("Chicago Blackhawks", "NHL"));
            source.Add(new Teams("Colorado Avalanche", "NHL"));
            source.Add(new Teams("Columbus Blue Jackets", "NHL"));
            source.Add(new Teams("Dallas Stars", "NHL"));
            source.Add(new Teams("Detroit Red Wings", "NHL"));
            source.Add(new Teams("Edmonton Oilers", "NHL"));
            source.Add(new Teams("Florida Panthers", "NHL"));
            source.Add(new Teams("Los Angeles Kings", "NHL"));
            source.Add(new Teams("Minnesota Wild", "NHL"));
            source.Add(new Teams("Montreal Canadiens", "NHL"));
            source.Add(new Teams("Nashville Predators", "NHL"));
            source.Add(new Teams("New Jersey Devils", "NHL"));
            source.Add(new Teams("New York Islanders", "NHL"));
            source.Add(new Teams("New York Rangers", "NHL"));
            source.Add(new Teams("Ottawa Senators", "NHL"));
            source.Add(new Teams("Philadelphia Flyers", "NHL"));
            source.Add(new Teams("Phoenix Coyotes", "NHL"));
            source.Add(new Teams("Pittsburgh Penguins", "NHL"));
            source.Add(new Teams("San Jose Sharks", "NHL"));
            source.Add(new Teams("St Louis Blues", "NHL"));
            source.Add(new Teams("Tampa Bay Lightning", "NHL"));
            source.Add(new Teams("Toronto Maple Leafs", "NHL"));
            source.Add(new Teams("Vancouver Canucks", "NHL"));
            source.Add(new Teams("Washington Capitals", "NHL"));
            source.Add(new Teams("Winnipeg Jets", "NHL"));
            source.Add(new Teams("Barrie Colts", "OHL"));
            source.Add(new Teams("Belleville Bulls", "OHL"));
            source.Add(new Teams("Erie Otters", "OHL"));
            source.Add(new Teams("Guelph Storm", "OHL"));
            source.Add(new Teams("Kingston Frontenacs", "OHL"));
            source.Add(new Teams("Kitchener Rangers", "OHL"));
            source.Add(new Teams("London Knights", "OHL"));
            source.Add(new Teams("Mississauga Steelheads", "OHL"));
            source.Add(new Teams("Niagara Ice Dogs", "OHL"));
            source.Add(new Teams("North Bay Battalion", "OHL"));
            source.Add(new Teams("Oshawa Generals", "OHL"));
            source.Add(new Teams("Ottawa 67s", "OHL"));
            source.Add(new Teams("Owen Sound Attack", "OHL"));
            source.Add(new Teams("Peterborough Petes", "OHL"));
            source.Add(new Teams("Plymouth Whalers", "OHL"));
            source.Add(new Teams("Saginaw Spirit", "OHL"));
            source.Add(new Teams("Sarnia Sting", "OHL"));
            source.Add(new Teams("Sault Ste Marie Greyhounds ", "OHL"));
            source.Add(new Teams("Sudbury Wolves", "OHL"));
            source.Add(new Teams("Windsor Spitfires", "OHL"));
            source.Add(new Teams("Acadie-Bathurst Titan", "QMJHL"));
            source.Add(new Teams("Baie-Comeau Drakkar", "QMJHL"));
            source.Add(new Teams("Blainville-Boisbriand Armada", "QMJHL"));
            source.Add(new Teams("Cape Breton Screaming Eagles", "QMJHL"));
            source.Add(new Teams("Charlottetown Islanders", "QMJHL"));
            source.Add(new Teams("Chicoutimi Sagueneens", "QMJHL"));
            source.Add(new Teams("Drummondville Voltigeurs", "QMJHL"));
            source.Add(new Teams("Gatineau Olympiques", "QMJHL"));
            source.Add(new Teams("Halifax Mooseheads", "QMJHL"));
            source.Add(new Teams("Moncton Wildcats", "QMJHL"));
            source.Add(new Teams("PEI Rocket", "QMJHL"));
            source.Add(new Teams("Quebec Remparts", "QMJHL"));
            source.Add(new Teams("Rimouski Oceanic", "QMJHL"));
            source.Add(new Teams("Rouyn-Noranda Huskies", "QMJHL"));
            source.Add(new Teams("Saint John Sea Dogs", "QMJHL"));
            source.Add(new Teams("Shawinigan Cataractes", "QMJHL"));
            source.Add(new Teams("Sherbrooke Phoenix", "QMJHL"));
            source.Add(new Teams("Val-dOr Foreurs", "QMJHL"));
            source.Add(new Teams("Victoriaville Tigres", "QMJHL"));
            source.Add(new Teams("Brandon Wheat Kings", "WHL"));
            source.Add(new Teams("Calgary Hitmen", "WHL"));
            source.Add(new Teams("Edmonton Oil Kings", "WHL"));
            source.Add(new Teams("Everett Silvertips", "WHL"));
            source.Add(new Teams("Kamloops Blazers", "WHL"));
            source.Add(new Teams("Kelowna Rockets", "WHL"));
            source.Add(new Teams("Kootenay Ice", "WHL"));
            source.Add(new Teams("Lethbridge Hurricanes", "WHL"));
            source.Add(new Teams("Medicine Hat Tigers", "WHL"));
            source.Add(new Teams("Moose Jaw Warriors", "WHL"));
            source.Add(new Teams("Portland Winterhawks", "WHL"));
            source.Add(new Teams("Prince Albert Raiders", "WHL"));
            source.Add(new Teams("Prince George Cougars", "WHL"));
            source.Add(new Teams("Red Deer Rebels", "WHL"));
            source.Add(new Teams("Regina Pats", "WHL"));
            source.Add(new Teams("Saskatoon Blades", "WHL"));
            source.Add(new Teams("Seattle Thunderbirds", "WHL"));
            source.Add(new Teams("Spokane Chiefs", "WHL"));
            source.Add(new Teams("Swift Current Broncos", "WHL"));
            source.Add(new Teams("Tri-City Americans", "WHL"));
            source.Add(new Teams("Vancouver Giants", "WHL"));
            source.Add(new Teams("Victoria Royals", "WHL"));
            List<AlphaKeyGroup<Teams>> DataSource = AlphaKeyGroup<Teams>.CreateGroups(source,
                System.Threading.Thread.CurrentThread.CurrentUICulture,
                (Teams s) => { return s.TeamName; }, true);
            teamPicker.ItemsSource = DataSource;
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += wc_DownloadStringCompletedHandler_2;
            wc.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemandDates?token=" + authToken));


        }

        void wc_DownloadStringCompletedHandler_2(object sender, DownloadStringCompletedEventArgs e)
        {
            JObject o = new JObject();
            try
            {
                o = JObject.Parse(e.Result);
            }
            catch (Exception)//System.Reflection.TargetInvocationException)
            {
                MessageBox.Show("No Dates found for OnDemand Videos");
                HideProgressIndicator();
            }

            foreach (JToken date in o["dates"])
            {
                goodDates.Add(date.ToString());
            }
            HideProgressIndicator();
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

        void wc_DownloadStringCompletedHandler(object sender, DownloadStringCompletedEventArgs e)
        {
            JObject o = new JObject();
            try
            {
                o = JObject.Parse(e.Result);
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
                ContentPanel.Height = 135 * o["schedule"].Count();

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
                HideProgressIndicator();

            }
            HideProgressIndicator();

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

        void ShowProgressIndicator()
        {
            _progressIndicator.IsVisible = true;
            _progressIndicator.IsIndeterminate = true;
        }

        void HideProgressIndicator()
        {
            _progressIndicator.IsVisible = false;
            _progressIndicator.IsIndeterminate = false;
        }

        private void liveDate_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            ShowProgressIndicator();
            int contentLength = ContentPanel.Children.Count;
            for(int child=0;child < contentLength;child++)
            {
                ContentPanel.Children.RemoveAt(0);

            }
            
            //GET TOKEN FROM MEMORY
            string authToken = (string)userSettings["Token"];

            //GET TODAYS LIVE GAMES
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += wc_DownloadStringCompletedHandler;
            wc.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetLive?date=" + liveDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&token=" + authToken));
        }

        private void ondemandDate_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            ShowProgressIndicator();
            //GET TOKEN FROM MEMORY
            string authToken = (string)userSettings["Token"];
            string favteam = (string)userSettings["FavTeam"];

            RemoveContent();

            //GET TODAYS LIVE GAMES
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += OnDemandDownloadCompleted;
            if (goodDates.Contains(ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy")))
            {
                if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == true))
                {
                    webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&team=" + btnTeam.Content + "&token=" + authToken));
                }
                else if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == false))
                {
                    webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&token=" + authToken));
                }
                else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == true))
                {
                    webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?team=" + btnTeam.Content + "&token=" + authToken));
                }
                else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == false))
                {
                    webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?&token=" + authToken));
                }
            }
            else
            {
                MessageBox.Show("There are no OnDemand Videos for that day.");
            }

        }

        private void pvtLivePivot_Loaded(object sender, RoutedEventArgs e)
        {
            ShowProgressIndicator();

            if (liveDate.Value.Value.Date.ToShortDateString() == DateTime.Now.Date.ToShortDateString())
            {
                //GET TOKEN FROM MEMORY
                string authToken = (string)userSettings["Token"];

                //GET TODAYS LIVE GAMES
                WebClient wc = new WebClient();
                wc.DownloadStringCompleted += wc_DownloadStringCompletedHandler;
                wc.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetLive?token=" + authToken));
            }
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ODContentPanel_Loaded(object sender, RoutedEventArgs e)
        {
            ShowProgressIndicator();
            //GET TOKEN FROM MEMORY
            string authToken = (string)userSettings["Token"];
            string favteam = (string)userSettings["FavTeam"];

            //GET TODAYS LIVE GAMES
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += OnDemandDownloadCompleted;

            if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == true))
            {
                if (goodDates.Contains(ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy")))
                {
                    webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&team=" + btnTeam.Content + "&token=" + authToken));
                }
            }
            else if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == false))
            {
                if (goodDates.Contains(ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy")))
                {
                    webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&token=" + authToken));
                }
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == true))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?team=" + btnTeam.Content + "&token=" + authToken));
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == false))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?&token=" + authToken));
            }
        }
        void OnDemandDownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            JObject o = new JObject();
            try
            {
                o = JObject.Parse(e.Result);
            }
            catch (Exception)//System.Reflection.TargetInvocationException)
            {
                //MessageBox.Show("No OnDemand games found matching these criteria.");
                HideProgressIndicator();
            }

            try
            {
                Button[] btnGames = new Button[o["ondemand"].Count()];
                TextBlock[] txtInfo = new TextBlock[o["ondemand"].Count()];
                int heightMargin = 0;
                int horizMargin = 0;
                int i = 0;
                ODContentPanel.Height = 135 * o["ondemand"].Count();

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
                HideProgressIndicator();
            }
            HideProgressIndicator();
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
            ShowProgressIndicator();
            Teams s = teamPicker.SelectedItem as Teams;
            btnTeam.Content = s.TeamName;
            teamPicker.Visibility = System.Windows.Visibility.Collapsed;
            ODContentPanel.Visibility = System.Windows.Visibility.Visible;
            //GET TOKEN FROM MEMORY
            string authToken = (string)userSettings["Token"];
            string favteam = (string)userSettings["FavTeam"];

            RemoveContent();
            
            //GET TODAYS LIVE GAMES
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += OnDemandDownloadCompleted;
            if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == true))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&team=" + btnTeam.Content + "&token=" + authToken));
            }
            else if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == false))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&token=" + authToken));
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == true))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?team=" + btnTeam.Content + "&token=" + authToken));
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == false))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?&token=" + authToken));
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
            ShowProgressIndicator();
            string authToken = (string)userSettings["Token"];
            string favteam = (string)userSettings["FavTeam"];

            RemoveContent();

            //GET TODAYS LIVE GAMES
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += OnDemandDownloadCompleted;
            if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == true))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&team=" + btnTeam.Content + "&token=" + authToken));
            }
            else if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == false))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&token=" + authToken));
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == true))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?team=" + btnTeam.Content + "&token=" + authToken));
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == false))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?&token=" + authToken));
            }
        }


        private void chkDate_Unchecked(object sender, RoutedEventArgs e)
        {
            ShowProgressIndicator();
            ondemandDate.IsEnabled = false;
            string authToken = (string)userSettings["Token"];
            string favteam = (string)userSettings["FavTeam"];

            RemoveContent();

            //GET TODAYS LIVE GAMES
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += OnDemandDownloadCompleted;
            if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == true))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&team=" + btnTeam.Content + "&token=" + authToken));
            }
            else if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == false))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&token=" + authToken));
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == true))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?team=" + btnTeam.Content + "&token=" + authToken));
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == false))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?&token=" + authToken));
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
           ShowProgressIndicator();
           btnTeam.IsEnabled = true;
           string authToken = (string)userSettings["Token"];
           string favteam = (string)userSettings["FavTeam"];

           RemoveContent();

           //GET TODAYS LIVE GAMES
           WebClient webClient = new WebClient();
           webClient.DownloadStringCompleted += OnDemandDownloadCompleted;
            if ((chkDate.IsChecked==true) && (chkTeam.IsChecked==true))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&team=" + btnTeam.Content + "&token=" + authToken));
            }
            else if ((chkDate.IsChecked==true) && (chkTeam.IsChecked==false))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy")+"&token=" + authToken));
            }
            else if ((chkDate.IsChecked==false)&&(chkTeam.IsChecked==true))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?team=" + btnTeam.Content + "&token=" + authToken));
            }
            else if ((chkDate.IsChecked == false)&& (chkTeam.IsChecked ==false))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?&token=" + authToken));
            }
        }

        private void chkTeam_Unchecked(object sender, RoutedEventArgs e)
        {
            ShowProgressIndicator();
            btnTeam.IsEnabled = false;
            string authToken = (string)userSettings["Token"];
            string favteam = (string)userSettings["FavTeam"];

            RemoveContent();

            //GET TODAYS LIVE GAMES
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += OnDemandDownloadCompleted;
            if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == true))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&team=" + btnTeam.Content + "&token=" + authToken));
            }
            else if ((chkDate.IsChecked == true) && (chkTeam.IsChecked == false))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?date=" + ondemandDate.Value.Value.Date.ToString("MM/dd/yyyy") + "&token=" + authToken));
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == true))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?team=" + btnTeam.Content + "&token=" + authToken));
            }
            else if ((chkDate.IsChecked == false) && (chkTeam.IsChecked == false))
            {
                webClient.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemand?&token=" + authToken));
            }
        }


        

    }
}