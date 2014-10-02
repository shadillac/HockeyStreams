﻿using System;
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

namespace HlsView
{
    public partial class PivotMain : PhoneApplicationPage
    {
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;

        public PivotMain()
        {
            InitializeComponent();
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
                MessageBox.Show("Username or Password Incorrect.  Please verify and reenter.");
            }
            txtOutput.Text = o.ToString();
        }

        private void liveDate_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            //GET TOKEN FROM MEMORY
            string authToken = (string)userSettings["Token"];

            //GET TODAYS LIVE GAMES
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += wc_DownloadStringCompletedHandler;
            wc.DownloadStringAsync(new Uri("https://api.hockeystreams.com/GetLive?date="+liveDate.Value.Value.Date.ToShortDateString()+"&token=" + authToken));
        }

    }
}