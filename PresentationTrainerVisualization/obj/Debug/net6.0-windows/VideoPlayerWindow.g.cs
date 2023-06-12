﻿#pragma checksum "..\..\..\VideoPlayerWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1771E8A2AF32B0C748E2D4BD9B66BA1E4A4709C6"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using PresentationTrainerVisualization.windows;
using ScottPlot;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace PresentationTrainerVisualization.windows {
    
    
    /// <summary>
    /// VideoPlayerWindow
    /// </summary>
    public partial class VideoPlayerWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\VideoPlayerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal PresentationTrainerVisualization.windows.VideoPlayerWindow VideoPlayerWindowElement;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\VideoPlayerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MediaElement VideoPlayer;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\VideoPlayerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ListBoxActions;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\VideoPlayerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ListBoxSentences;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\VideoPlayerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.PackIcon playButtonIcon;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\VideoPlayerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LoopButton;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\VideoPlayerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider VolumeSlider;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\VideoPlayerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider speedRatioSlider;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.4.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/PresentationTrainerVisualization;component/videoplayerwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\VideoPlayerWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.4.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.VideoPlayerWindowElement = ((PresentationTrainerVisualization.windows.VideoPlayerWindow)(target));
            return;
            case 2:
            this.VideoPlayer = ((System.Windows.Controls.MediaElement)(target));
            
            #line 18 "..\..\..\VideoPlayerWindow.xaml"
            this.VideoPlayer.MediaEnded += new System.Windows.RoutedEventHandler(this.MediaElement_MediaEnded);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ListBoxActions = ((System.Windows.Controls.ListBox)(target));
            
            #line 27 "..\..\..\VideoPlayerWindow.xaml"
            this.ListBoxActions.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.HandleListBox);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ListBoxSentences = ((System.Windows.Controls.ListBox)(target));
            
            #line 39 "..\..\..\VideoPlayerWindow.xaml"
            this.ListBoxSentences.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.HandleListBox);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 54 "..\..\..\VideoPlayerWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OnButtonClickPlayMedia);
            
            #line default
            #line hidden
            return;
            case 6:
            this.playButtonIcon = ((MaterialDesignThemes.Wpf.PackIcon)(target));
            return;
            case 7:
            
            #line 57 "..\..\..\VideoPlayerWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.LeftButtonClicked);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 60 "..\..\..\VideoPlayerWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RightButtonClicked);
            
            #line default
            #line hidden
            return;
            case 9:
            this.LoopButton = ((System.Windows.Controls.Button)(target));
            
            #line 63 "..\..\..\VideoPlayerWindow.xaml"
            this.LoopButton.Click += new System.Windows.RoutedEventHandler(this.LoopButtonClicked);
            
            #line default
            #line hidden
            return;
            case 10:
            this.VolumeSlider = ((System.Windows.Controls.Slider)(target));
            
            #line 68 "..\..\..\VideoPlayerWindow.xaml"
            this.VolumeSlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.ChangeMediaVolume);
            
            #line default
            #line hidden
            return;
            case 11:
            this.speedRatioSlider = ((System.Windows.Controls.Slider)(target));
            
            #line 70 "..\..\..\VideoPlayerWindow.xaml"
            this.speedRatioSlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.ChangeMediaSpeedRatio);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

