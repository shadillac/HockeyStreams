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
    public partial class LiveStreamDetail : PhoneApplicationPage
    {
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;

        public LiveStreamDetail()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //GET TOKEN FROM MEMORY
            string authToken = (string)userSettings["Token"];
            string streamID;
            NavigationContext.QueryString.TryGetValue("streamID", out streamID);

            string location;
            try
            {
                location = (string)userSettings["Location"];
            }
            catch (Exception)
            {
                location = "North America - Central";
            }


            //GET TODAYS LIVE GAMES
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += wc_DownloadStringCompletedHandler;
            wc.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetLiveStream?id=" + streamID + "&location="+ location + "&token=" + authToken));

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
                MessageBox.Show("Game Not Found.");
            }

            try
            {
                awayText.Text = o["awayTeam"].ToString();
                atText.Text = "@";
                homeText.Text = o["homeTeam"].ToString();
                txtGameTime.Text = "Start Time: " + o["startTime"].ToString();
                awayScore.Text = o["awayScore"].ToString();
                homeScore.Text = o["homeScore"].ToString();
                Button launchStream = new Button { Content = "Launch Live Stream", Tag = o["nonDVR"][0]["src"].ToString(), Margin=new Thickness(20,482,0,0), VerticalAlignment=VerticalAlignment.Top, Width=324, Height=105 };
                launchStream.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                
                launchStream.Click += launchStream_Click;
                ContentPanel.Children.Add(launchStream);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void launchStream_Click(object sender, RoutedEventArgs e)
        {
            Button target = sender as Button;
            NavigationService.Navigate(new Uri("/StreamViewer.xaml?source=" + target.Tag.ToString(), UriKind.Relative));
        }
    }
}