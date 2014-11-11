using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace HlsView
{
    public partial class HighlightViewer : PhoneApplicationPage
    {
        static readonly TimeSpan FFStepSize = TimeSpan.FromSeconds(30);
        static readonly TimeSpan RewStepSize = TimeSpan.FromSeconds(5);

        public HighlightViewer()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //this.navigationHelper.OnNavigatedTo(e);
            string source = "";
            NavigationContext.QueryString.TryGetValue("source", out source);
            mdaHighView.Source = new Uri(source, UriKind.Absolute);
            //btnPlay.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mdaHighView.Play();
            //btnPlay.IsEnabled = false;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mdaHighView.Stop();
            btnPlay.IsEnabled = true;
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (mdaHighView.CanPause)
            {
                mdaHighView.Pause();
                btnPlay.IsEnabled = true;
            }
        }

        private void btnRew_Click(object sender, RoutedEventArgs e)
        {
            if (null == mdaHighView || mdaHighView.CurrentState != MediaElementState.Playing)
                return;
            var position = mdaHighView.Position;

            if (position < RewStepSize)
                position = TimeSpan.Zero;
            else
                position -= RewStepSize;

            mdaHighView.Position = position - RewStepSize;

        }

        private void btnFF_Click(object sender, RoutedEventArgs e)
        {
            if (null == mdaHighView || mdaHighView.CurrentState != MediaElementState.Playing)
                return;
            var position = mdaHighView.Position;
            mdaHighView.Position = position + FFStepSize;
        }

        private void mdaHighView_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            tbStatus.Text = mdaHighView.CurrentState.ToString();
        }

        private void mdaHighView_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (SystemTray.IsVisible)
            {
                CollapseButtons();
                SystemTray.IsVisible = false;
                mdaHighView.Height = 475;
                mdaHighView.Width = 800;
                mdaHighView.Margin = new Thickness(-167.596, 125, -170, 160.766);
                mdaHighView.Stretch = Stretch.Fill;
            }
            else
            {
                mdaHighView.Height = 399.593;
                mdaHighView.Width = 754.913;
                mdaHighView.Margin = new Thickness(-111.447, 175, -185, 179.319);
                mdaHighView.Stretch = Stretch.Fill;
                UncollapseButtons();
                SystemTray.IsVisible = true;
            }
        }
        private void CollapseButtons()
        {
            btnPlay.Visibility = Visibility.Collapsed;
            btnPause.Visibility = Visibility.Collapsed;
            btnFF.Visibility = Visibility.Collapsed;
            btnRew.Visibility = Visibility.Collapsed;
            tbStatus.Visibility = Visibility.Collapsed;
        }
        private void UncollapseButtons()
        {
            btnPlay.Visibility = Visibility.Visible;
            btnPause.Visibility = Visibility.Visible;
            btnFF.Visibility = Visibility.Visible;
            btnRew.Visibility = Visibility.Visible;
            tbStatus.Visibility = Visibility.Visible;
        }
    }
}