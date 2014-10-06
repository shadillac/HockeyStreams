using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HlsView.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.IsolatedStorage;

namespace HlsView
{
    public partial class Login : PhoneApplicationPage
    {
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;
        private ProgressIndicator _progressIndicator = new ProgressIndicator(); //Create progress indicator to indicate system busy.

        public Login()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            
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
                //MessageBox.Show("Username or Password Incorrect.  Please verify and reenter.");
                btnLogin.IsEnabled = true;
                _progressIndicator.IsVisible = false;
                _progressIndicator.IsIndeterminate = false;
            }

            if ((string)o["status"]=="Success")
            {
                if ((string)o["membership"]=="Premium")
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
                    //ADD TOKEN TO USER STORAGE
                    try
                    {
                        userSettings.Add("FavTeam", (string)o["favteam"]);
                    }
                    catch (ArgumentException)
                    {
                        userSettings["FavTeam"] = (string)o["favteam"];
                    }
                    
                    //GOOD ACCOUNT - AUTHENTICATE AND NAVIGATE.
                    NavigationService.Navigate(new Uri("/PivotMain.xaml", UriKind.Relative));
                    _progressIndicator.IsVisible = false;
                    _progressIndicator.IsIndeterminate = false;
       
                }
                else
                {
                    MessageBox.Show("Your membership is not a premium membership.  Premium Membership is needed in order to access content.");
                    btnLogin.IsEnabled = true;
                    _progressIndicator.IsVisible = false;
                    _progressIndicator.IsIndeterminate = false;
                }
            }
            else
            {
                MessageBox.Show("Username or Password incorrect.  Please verify and enter again.");
                btnLogin.IsEnabled = true;
                _progressIndicator.IsVisible = false;
                _progressIndicator.IsIndeterminate = false;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //Create progress indicator.
            _progressIndicator.IsVisible = true;
            _progressIndicator.IsIndeterminate = true;
            _progressIndicator.Text = "Logging In";
            SystemTray.SetProgressIndicator(this, _progressIndicator);
            btnLogin.IsEnabled = false;

            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            wc.UploadStringCompleted += wc_UploadStringCompleted;
            string parameters = "username="+txtUsername.Text+"&password="+pwdPassword.Password+"&key=" + AppResources.APIKey.ToString();
            wc.UploadStringAsync(new Uri("https://api.hockeystreams.com/Login?"), "POST", parameters);
        }

    }
}