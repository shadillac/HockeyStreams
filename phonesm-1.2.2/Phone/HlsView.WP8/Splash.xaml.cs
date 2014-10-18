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
using Microsoft.Phone.Tasks;
using HlsView.Resources;
using Newtonsoft.Json.Linq;

namespace HlsView
{
    public partial class Splash : PhoneApplicationPage
    {
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;
        public Splash()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread.Sleep(1000);

            try
            {
                string username = (string)userSettings["Username"];
                string password = (string)userSettings["Password"];


                WebClient wc = new WebClient();
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.UploadStringCompleted += wc_UploadStringCompleted;
                string parameters = "username=" + username + "&password=" + password + "&key=" + AppResources.APIKey.ToString();
                wc.UploadStringAsync(new Uri("https://api.hockeystreams.com/Login?"), "POST", parameters);



            }
            catch (KeyNotFoundException)
            {
                NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
            }
        }

        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            JObject o = new JObject();
            try
            {
                o = JObject.Parse(e.Result);
            }
            catch (System.Reflection.TargetInvocationException)
            {
            }

            if ((string)o["status"] == "Success")
            {
                if ((string)o["membership"] == "Premium")
                {
                    //ADD TOKEN TO USER STORAGE
                    try
                    {
                        userSettings.Add("Token", (string)o["token"]);
                    }
                    catch (ArgumentException)
                    {
                        userSettings["Token"] = (string)o["token"];
                    }
                    NavigationService.Navigate(new Uri("/PivotMain.xaml", UriKind.Relative));

                }
                else
                {
                    MessageBox.Show("You must have a premium account to log into HockeyStreams.com");
                    NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
                }
            }
            else
            {
                NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
            }
        }
    }
}