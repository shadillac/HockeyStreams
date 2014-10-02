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
            catch (Exception ex)//System.Reflection.TargetInvocationException)
            {
                MessageBox.Show(ex.Message);
            }

            Button[] btnAway = new Button[o["schedule"].Count()];
            int heightMargin = 0;
            int horizMargin = 0;
            int i = 0;

            foreach (JToken game in o["schedule"])
            {
                btnAway[i] = new Button { Content = game["awayTeam"].ToString() + " @ " + game["homeTeam"].ToString(), FontSize = 14, Tag = game["id"].ToString() };
                btnAway[i].VerticalAlignment = System.Windows.VerticalAlignment.Top;
                btnAway[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                btnAway[i].Margin = new Thickness(horizMargin + 0, heightMargin - 0, 0, 0);
                btnAway[i].Width = 450;
                if (game["isPlaying"].ToString()=="1")
                {
                    btnAway[i].Background = GetColorFromHexa("#FFFF00");
                    btnAway[i].Foreground = GetColorFromHexa("#000000");
                }
                ContentPanel.Children.Add(btnAway[i]);

                heightMargin = heightMargin + 55;
                i++;
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