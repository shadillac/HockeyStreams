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
using System.Windows.Media;

namespace HlsView
{
    public partial class OnDemandDetail : PhoneApplicationPage
    {
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;
        private ProgressIndicator _progressIndicator = new ProgressIndicator(); //Create progress indicator to indicate system busy.

        public OnDemandDetail()
        {
            InitializeComponent();
        }

        private void RemoveContent()
        {
            int contentLength = ContentPanel.Children.Count;
            for (int child = 0; child < contentLength; child++)
            {
                ContentPanel.Children.RemoveAt(0);
            }
        }

        void ShowProgressIndicator()
        {
            _progressIndicator.IsVisible = true;
            _progressIndicator.IsIndeterminate = false;
        }

        void HideProgressIndicator()
        {
            _progressIndicator.IsVisible = false;
            _progressIndicator.IsIndeterminate = true;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            _progressIndicator.Text = "Getting OnDemand Details";
            SystemTray.SetProgressIndicator(this, _progressIndicator);
            ShowProgressIndicator();

            RemoveContent();

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
            wc.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetOnDemandStream?id=" + streamID + "&location=" + location + "&token=" + authToken));

        }

        void wc_DownloadStringCompletedHandler(object sender, DownloadStringCompletedEventArgs e)
        {
            string authToken = (string)userSettings["Token"];
            JObject o = new JObject();
            try
            {
                o = JObject.Parse(e.Result);
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
                txtGameTime.Text = "Event Type: " + o["event"].ToString();

                try
                {
                    //find iStream for full game.
                    foreach(JToken iStream in o["SDstreams"])
                    {
                        if (iStream["type"].ToString() == "iStream")
                        {
                            if (iStream["src"].ToString() != "")
                            {
                                Button launchStream = new Button { Content = "Full Game", Tag = iStream["src"].ToString(), Margin = new Thickness(80, 10, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 324, Height = 105 };
                                launchStream.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                launchStream.Click += launchStream_Click;
                                ContentPanel.Children.Add(launchStream);
                            }
                            else
                            {
                                Button launchStream = new Button { Content = "Full Game Not Available", Margin = new Thickness(55, 10, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 380, Height = 105 };
                                launchStream.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                launchStream.Background = GetColorFromHexa("#FF0000");
                                launchStream.Foreground = GetColorFromHexa("#000000");
                                ContentPanel.Children.Add(launchStream);
                            }
                            
                        }
                    }
                }
                catch (Exception)
                {
                    Button launchStream = new Button { Content = "Full Game Not Available", Margin = new Thickness(55, 10, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 380, Height = 105 };
                    launchStream.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    launchStream.Background = GetColorFromHexa("#FF0000");
                    launchStream.Foreground = GetColorFromHexa("#000000");
                    ContentPanel.Children.Add(launchStream);
                    HideProgressIndicator();
                }
                

                try
                {
                    if (o["highlights"][0]["awaySrc"].ToString() != "")
                    {
                        Button awayHighlights = new Button { Content = "Away Highlights", Tag = o["highlights"][0]["awaySrc"].ToString(), Margin = new Thickness(80, 120, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 324, Height = 105 };
                        awayHighlights.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        awayHighlights.Click += launchStream_Click;
                        ContentPanel.Children.Add(awayHighlights);
                    }
                    else
                    {
                        Button awayHighlights = new Button { Content = "Away Highlights Not Available", Margin = new Thickness(55, 120, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 380, Height = 105 };
                        awayHighlights.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        awayHighlights.Background = GetColorFromHexa("#FF0000");
                        awayHighlights.Foreground = GetColorFromHexa("#000000");
                        ContentPanel.Children.Add(awayHighlights);
                    }
                    
                }
                catch (Exception)
                {
                    Button awayHighlights = new Button { Content = "Away Highlights Not Available", Margin = new Thickness(55, 120, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 380, Height = 105 };
                    awayHighlights.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    awayHighlights.Background = GetColorFromHexa("#FF0000");
                    awayHighlights.Foreground = GetColorFromHexa("#000000");
                    ContentPanel.Children.Add(awayHighlights);
                    
                }

                try
                {
                    if (o["highlights"][0]["homeSrc"].ToString() != "")
                    {
                        Button homeHighlights = new Button { Content = "Home Highlights", Tag = o["highlights"][0]["homeSrc"].ToString(), Margin = new Thickness(80, 230, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 324, Height = 105 };
                        homeHighlights.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        homeHighlights.Click += launchStream_Click;
                        ContentPanel.Children.Add(homeHighlights);
                    }
                    else
                    {
                        Button homeHighlights = new Button { Content = "Home Highlights Not Available", Margin = new Thickness(55, 230, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 380, Height = 105 };
                        homeHighlights.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        homeHighlights.Background = GetColorFromHexa("#FF0000");
                        homeHighlights.Foreground = GetColorFromHexa("#000000");
                        ContentPanel.Children.Add(homeHighlights);
                    }
                    
                }
                catch (Exception)
                {
                    Button homeHighlights = new Button { Content = "Home Highlights Not Available", Margin = new Thickness(55, 230, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 380, Height = 105 };
                    homeHighlights.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    homeHighlights.Background = GetColorFromHexa("#FF0000");
                    homeHighlights.Foreground = GetColorFromHexa("#000000");
                    ContentPanel.Children.Add(homeHighlights);
                }

                try
                {
                    if (o["condensed"][0]["awaySrc"].ToString() != "")
                    {
                        Button awayCondensed = new Button { Content = "Away Condensed Game", Tag = o["condensed"][0]["awaySrc"].ToString(), Margin = new Thickness(80, 340, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 324, Height = 105 };
                        awayCondensed.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        awayCondensed.Click += launchStream_Click;
                        ContentPanel.Children.Add(awayCondensed);
                    }
                    else
                    {
                        Button awayCondensed = new Button { Content = "Away Condensed Not Available", Margin = new Thickness(55, 340, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 380, Height = 105 };
                        awayCondensed.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        awayCondensed.Background = GetColorFromHexa("#FF0000");
                        awayCondensed.Foreground = GetColorFromHexa("#000000");
                        ContentPanel.Children.Add(awayCondensed);
                    }
                }
                catch (Exception)
                {
                    Button awayCondensed = new Button { Content = "Away Condensed Not Available", Margin = new Thickness(55, 340, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 380, Height = 105 };
                    awayCondensed.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    awayCondensed.Background = GetColorFromHexa("#FF0000");
                    awayCondensed.Foreground = GetColorFromHexa("#000000");
                    ContentPanel.Children.Add(awayCondensed);
                }

                try
                {
                    if (o["condensed"][0]["homeSrc"].ToString() != "")
                    {
                        Button homeCondensed = new Button { Content = "Home Condensed Game", Tag = o["condensed"][0]["homeSrc"].ToString(), Margin = new Thickness(80, 450, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 324, Height = 105 };
                        homeCondensed.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        homeCondensed.Click += launchStream_Click;
                        ContentPanel.Children.Add(homeCondensed);
                    }
                    else
                    {
                        Button homeCondensed = new Button { Content = "Home Condensed Not Available", Margin = new Thickness(55, 450, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 380, Height = 105 };
                        homeCondensed.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        homeCondensed.Background = GetColorFromHexa("#FF0000");
                        homeCondensed.Foreground = GetColorFromHexa("#000000");
                        ContentPanel.Children.Add(homeCondensed);
                    }
                }
                catch (Exception)
                {
                    Button homeCondensed = new Button { Content = "Home Condensed Not Available", Margin = new Thickness(55, 450, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 380, Height = 105 };
                    homeCondensed.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    homeCondensed.Background = GetColorFromHexa("#FF0000");
                    homeCondensed.Foreground = GetColorFromHexa("#000000");
                    ContentPanel.Children.Add(homeCondensed);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        void launchStream_Click(object sender, RoutedEventArgs e)
        {
            Button target = sender as Button;
            if (target.Tag.ToString().Contains(".m3u8"))
            {
                NavigationService.Navigate(new Uri("/StreamViewer.xaml?source=" + target.Tag.ToString(), UriKind.Relative));
            }
            else if (target.Tag.ToString().EndsWith(".mp4"))
            {
                NavigationService.Navigate(new Uri("/HighlightViewer.xaml?source=" + target.Tag.ToString(), UriKind.Relative));
            }
            
        }
    }
}