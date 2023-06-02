﻿#pragma checksum "..\..\..\..\Pages\ChartsConfigurationPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A9CA058E29448E191E7675B65C42585CB09953CF"
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
using PresentationTrainerVisualization.Pages;
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


namespace PresentationTrainerVisualization.Pages {
    
    
    /// <summary>
    /// ChartsConfigurationPage
    /// </summary>
    public partial class ChartsConfigurationPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 45 "..\..\..\..\Pages\ChartsConfigurationPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton DaysOrSessions;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\Pages\ChartsConfigurationPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider NumberOfX;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\..\Pages\ChartsConfigurationPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker StartDate;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\..\Pages\ChartsConfigurationPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker EndDate;
        
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
            System.Uri resourceLocater = new System.Uri("/PresentationTrainerVisualization;component/pages/chartsconfigurationpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\ChartsConfigurationPage.xaml"
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
            this.DaysOrSessions = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 45 "..\..\..\..\Pages\ChartsConfigurationPage.xaml"
            this.DaysOrSessions.Checked += new System.Windows.RoutedEventHandler(this.HandleLastSessionsToggle);
            
            #line default
            #line hidden
            return;
            case 2:
            this.NumberOfX = ((System.Windows.Controls.Slider)(target));
            return;
            case 3:
            
            #line 54 "..\..\..\..\Pages\ChartsConfigurationPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SaveButton1Clicked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.StartDate = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 5:
            this.EndDate = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 6:
            
            #line 87 "..\..\..\..\Pages\ChartsConfigurationPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SaveButton2Clicked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

