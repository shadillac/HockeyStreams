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
using System.Net.Http;

namespace HlsView
{
    public partial class LiveStreamDetail : PhoneApplicationPage
    {
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;
        private ProgressIndicator _progressIndicator = new ProgressIndicator(); //Create progress indicator to indicate system busy.

        public LiveStreamDetail()
        {
            InitializeComponent();
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

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //set progress indicator
            _progressIndicator.Text = "Getting Stream Details";
            SystemTray.SetProgressIndicator(this, _progressIndicator);
            ShowProgressIndicator();

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

            string scores;
            try
            {
                scores = (string)userSettings["HideScores"];
            }
            catch (Exception)
            {
                scores = "0";
            }


            GetLiveGame(streamID, location, authToken, scores);

        }

        private async void GetLiveGame(string streamID, string location, string authToken, string scores)
        {
            HttpClient hc = new HttpClient();
            string response = await hc.GetStringAsync("https://api.hockeystreams.com/GetLiveStream?id=" + streamID + "&location=" + location + "&token=" + authToken);

            JObject o = new JObject();
            try
            {
                o = JObject.Parse(response);
            }
            catch (Exception)//System.Reflection.TargetInvocationException)
            {
                MessageBox.Show("Game Not Found.");
                HideProgressIndicator();
            }

            try
            {
                awayText.Text = o["awayTeam"].ToString();
                atText.Text = "@";
                homeText.Text = o["homeTeam"].ToString();
                txtGameTime.Text = "Start Time: " + o["startTime"].ToString();
                
                //Hide scores if settings dictate
                if (scores == "1")
                {
                    tbAwayScore.Visibility = Visibility.Collapsed;
                    tbHomeScore.Visibility = Visibility.Collapsed;
                    awayScore.Visibility = Visibility.Collapsed;
                    homeScore.Visibility = Visibility.Collapsed;
                    tbHiddenInfo.Visibility = Visibility.Visible;
                    awayScore.Text = o["awayScore"].ToString();
                    homeScore.Text = o["homeScore"].ToString();
                }
                else
                {
                    tbAwayScore.Visibility = Visibility.Visible;
                    tbHomeScore.Visibility = Visibility.Visible;
                    awayScore.Visibility = Visibility.Visible;
                    homeScore.Visibility = Visibility.Visible;
                    tbHiddenInfo.Visibility = Visibility.Collapsed;
                    awayScore.Text = o["awayScore"].ToString();
                    homeScore.Text = o["homeScore"].ToString();
                }
                Button launchStream = new Button { Content = "Launch Live Stream", Tag = o["nonDVRSD"][0]["src"].ToString(), Margin = new Thickness(5, 300, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 324, Height = 105 };
                launchStream.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                
                launchStream.Click += launchStream_Click;
                ContentPanel.Children.Add(launchStream);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                HideProgressIndicator();
            }
            HideProgressIndicator();
        }

        void launchStream_Click(object sender, RoutedEventArgs e)
        {
            Button target = sender as Button;
            NavigationService.Navigate(new Uri("/StreamViewer.xaml?source=" + target.Tag.ToString(), UriKind.Relative));
        }
    }
}