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
using Microsoft.Phone.Notification;
using System.Text;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;

namespace HlsView
{
    public class PushItems
    {
        public string Id { get; set; }
        public string usrname { get; set; }
        public string channeluri { get; set; }
        public string gameid { get; set; }
    }

    public partial class Splash : PhoneApplicationPage
    {
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;
        private string channelURI = "";
        //private MobileServiceCollection<PushItems, PushItems> items;
        private IMobileServiceTable<PushItems> PushTable = App.MobileService.GetTable<PushItems>();
        private MobileServiceCollection<PushItems, PushItems> pushdata;

        public Splash()
        {
            InitializeComponent();

            /// Holds the push channel that is created or found.
            HttpNotificationChannel pushChannel;

            // The name of our push channel.
            string channelName = "ToastSampleChannel";

            InitializeComponent();

            // Try to find the push channel.
            pushChannel = HttpNotificationChannel.Find(channelName);

            // If the channel was not found, then create a new connection to the push service.
            if (pushChannel == null)
            {
                pushChannel = new HttpNotificationChannel(channelName);

                // Register for all the events before attempting to open the channel.
                pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                // Register for this notification only if you need to receive the notifications while your application is running.
                pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

                pushChannel.Open();

                // Bind this new channel for toast events.
                pushChannel.BindToShellToast();

            }
            else
            {
                // The channel was already open, so just register for all the events.
                pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                // Register for this notification only if you need to receive the notifications while your application is running.
                pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

                // Display the URI for testing purposes. Normally, the URI would be passed back to your web service at this point.
                System.Diagnostics.Debug.WriteLine(pushChannel.ChannelUri.ToString());
                //MessageBox.Show(String.Format("Channel Uri is {0}",
                //    pushChannel.ChannelUri.ToString()));

            }

        }

        void PushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            channelURI = e.ChannelUri.ToString();

            Dispatcher.BeginInvoke(() =>
            {
                // Display the new URI for testing purposes.   Normally, the URI would be passed back to your web service at this point.
                System.Diagnostics.Debug.WriteLine(e.ChannelUri.ToString());
                //MessageBox.Show(String.Format("Channel Uri is {0}",
                //    e.ChannelUri.ToString()));

            });
        }

        void PushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            // Error handling logic for your particular application would be here.
            Dispatcher.BeginInvoke(() =>
                MessageBox.Show(String.Format("A push notification {0} error occurred.  {1} ({2}) {3}",
                    e.ErrorType, e.Message, e.ErrorCode, e.ErrorAdditionalData))
                    );
        }

        void PushChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            StringBuilder message = new StringBuilder();
            string relativeUri = string.Empty;

            //message.AppendFormat("Received Toast {0}:\n", DateTime.Now.ToShortTimeString());

            // Parse out the information that was part of the message.
            foreach (string key in e.Collection.Keys)
            {
                //message.AppendFormat("{0}: {1}\n", key, e.Collection[key]);
                
                if (string.Compare(
                    key,
                    "wp:Param",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.CompareOptions.IgnoreCase) == 0)
                {
                    relativeUri = e.Collection[key];
                }
                else
                {
                    message.AppendFormat("{0}\n", e.Collection[key]);
                }
            }

            // Display a dialog of all the fields in the toast.
            Dispatcher.BeginInvoke(() => MessageBox.Show(message.ToString()));

        }


        private async void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                string username = (string)userSettings["Username"];
                string password = (string)userSettings["Password"];

                pushdata = await PushTable
                    .Where(Item => Item.usrname == username)
                    .ToCollectionAsync();

                if (pushdata.Count > 0)
                {
                    foreach (PushItems pdata in pushdata)
                    {
                        pdata.channeluri = channelURI;
                        await PushTable.UpdateAsync(pushdata[0]); 
                    }                    
                }
                else
                {
                    PushItems item = new PushItems { usrname = username, channeluri = channelURI };
                    await App.MobileService.GetTable<PushItems>().InsertAsync(item);
                }

                try
                {
                    userSettings.Add("ChannelURI", channelURI);
                }
                catch (ArgumentException)
                {
                    userSettings["ChannelURI"] = channelURI;
                }

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