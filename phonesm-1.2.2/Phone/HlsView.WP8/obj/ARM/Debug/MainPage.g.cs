﻿#pragma checksum "C:\Users\shmorris\Documents\GitHub\HockeyStreams\phonesm-1.2.2\Phone\HlsView.WP8\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C98987E7B3A62C6500D839089B429F42"
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


namespace HlsView {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.Button playButton;
        
        internal System.Windows.Controls.Button stopButton;
        
        internal System.Windows.Controls.Button wakeButton;
        
        internal System.Windows.Controls.TextBlock PositionBox;
        
        internal System.Windows.Controls.TextBlock MediaStateBox;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock errorBox;
        
        internal System.Windows.Controls.MediaElement mediaElement1;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/HlsView8;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.playButton = ((System.Windows.Controls.Button)(this.FindName("playButton")));
            this.stopButton = ((System.Windows.Controls.Button)(this.FindName("stopButton")));
            this.wakeButton = ((System.Windows.Controls.Button)(this.FindName("wakeButton")));
            this.PositionBox = ((System.Windows.Controls.TextBlock)(this.FindName("PositionBox")));
            this.MediaStateBox = ((System.Windows.Controls.TextBlock)(this.FindName("MediaStateBox")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.errorBox = ((System.Windows.Controls.TextBlock)(this.FindName("errorBox")));
            this.mediaElement1 = ((System.Windows.Controls.MediaElement)(this.FindName("mediaElement1")));
        }
    }
}

