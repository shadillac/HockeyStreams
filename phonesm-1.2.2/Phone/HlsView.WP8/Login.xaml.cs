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

namespace HlsView
{
    public partial class Login : PhoneApplicationPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            wc.UploadStringCompleted += wc_UploadStringCompleted;
            string parameters = "username=shadillac&password=Niggaplease9&key=" + AppResources.APIKey.ToString();
            wc.UploadStringAsync(new Uri("https://api.hockeystreams.com/Login?"),"POST", parameters);
        }

        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            JObject o = JObject.Parse(e.Result);
            tbOutput.Text = o.ToString();
        }

        private void HttpCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            JObject o = JObject.Parse(e.Result);
            tbOutput.Text = o.ToString();
        }
    }
}