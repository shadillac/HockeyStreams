﻿#pragma checksum "C:\Users\shmorris\Documents\GitHub\HockeyStreams\phonesm-1.2.2\Phone\BackgroundAudio.Sample.WP8\..\BackgroundAudio.Sample.WP7\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B758D0B2A066F463B8E63A9713B3FB4B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace BackgroundAudio.Sample.WP7 {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock txtState;
        
        internal System.Windows.Controls.TextBlock txtTrack;
        
        internal System.Windows.Controls.ProgressBar positionIndicator;
        
        internal System.Windows.Controls.TextBlock textPosition;
        
        internal System.Windows.Controls.TextBlock textRemaining;
        
        internal Microsoft.Phone.Shell.ApplicationBar AppBar;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/BackgroundAudio.Sample.WP8;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.txtState = ((System.Windows.Controls.TextBlock)(this.FindName("txtState")));
            this.txtTrack = ((System.Windows.Controls.TextBlock)(this.FindName("txtTrack")));
            this.positionIndicator = ((System.Windows.Controls.ProgressBar)(this.FindName("positionIndicator")));
            this.textPosition = ((System.Windows.Controls.TextBlock)(this.FindName("textPosition")));
            this.textRemaining = ((System.Windows.Controls.TextBlock)(this.FindName("textRemaining")));
            this.AppBar = ((Microsoft.Phone.Shell.ApplicationBar)(this.FindName("AppBar")));
        }
    }
}

