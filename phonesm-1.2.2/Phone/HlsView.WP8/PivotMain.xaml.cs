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

namespace HlsView
{
    public partial class PivotMain : PhoneApplicationPage
    {
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;

        public PivotMain()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
        }
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appbarmenuitem = new ApplicationBarMenuItem("Settings...");
            ApplicationBarMenuItem appbarmenuitem2 = new ApplicationBarMenuItem("About");
            ApplicationBar.MenuItems.Add(appbarmenuitem);
            ApplicationBar.MenuItems.Add(appbarmenuitem2);
            appbarmenuitem.Click += appbarmenuitem_Click;
            appbarmenuitem2.Click += appbarmenuitem2_Click;
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
            if(liveDate.Value.Value.Date.ToShortDateString()==DateTime.Now.Date.ToShortDateString())
            {
                //GET TOKEN FROM MEMORY
                string authToken = (string)userSettings["Token"];

                //GET TODAYS LIVE GAMES
                WebClient wc = new WebClient();
                wc.DownloadStringCompleted += wc_DownloadStringCompletedHandler;
                wc.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetLive?token=" + authToken));
            }
            
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
                MessageBox.Show("No games found for this day.");
            }

            try
            {
                Button[] btnGames = new Button[o["schedule"].Count()];
                TextBlock[] txtInfo = new TextBlock[o["schedule"].Count()];
                int heightMargin = 0;
                int horizMargin = 0;
                int i = 0;
                ContentPanel.Height = 70 * o["schedule"].Count();

                foreach (JToken game in o["schedule"])
                {
                    btnGames[i] = new Button { Content = game["awayTeam"].ToString() + " @ " + game["homeTeam"].ToString(), FontSize = 14, Tag = game["id"].ToString() };
                    btnGames[i].VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    btnGames[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    btnGames[i].Margin = new Thickness(horizMargin + 0, heightMargin - 0, 0, 0);
                    btnGames[i].Width = 450;
                    if (game["isPlaying"].ToString() == "1")
                    {
                        btnGames[i].Background = GetColorFromHexa("#FFFF00");
                        btnGames[i].Foreground = GetColorFromHexa("#000000");
                        btnGames[i].Click += GameList_Click;
                    }
                    
                    ContentPanel.Children.Add(btnGames[i]);

                    txtInfo[i] = new TextBlock { Text = "Start Time: " + game["startTime"].ToString() + " :: " + game["feedType"].ToString(),FontSize = 12 };
                    txtInfo[i].VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    txtInfo[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    txtInfo[i].Margin = new Thickness(horizMargin + 225, heightMargin + 50, 0, 0);
                    ContentPanel.Children.Add(txtInfo[i]);

                    heightMargin = heightMargin + 65;
                    i++;
                }
            }
            catch
            {

            }

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

        private void liveDate_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
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
            wc.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetLive?date="+liveDate.Value.Value.Date.ToShortDateString()+"&token=" + authToken));
        }

    }
}