﻿#pragma checksum "..\..\FormMain.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8B45645C77D3003EF89F545B6876C1AF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Controls.UserControls;
using MahApps.Metro.Controls;
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
using TqatProTrackingTool;
using TqatProViewModel;


namespace TqatProTrackingTool {
    
    
    /// <summary>
    /// FormMain
    /// </summary>
    public partial class FormMain : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 79 "..\..\FormMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelCompany;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\FormMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelUser;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\FormMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label labelDatabaseHost;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\FormMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Ribbon.RibbonMenuButton ribbonMenuButtonUser;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\FormMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Ribbon.RibbonMenuButton ribbonMenuButtonDisplayMember;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\FormMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Ribbon.RibbonGallery ribbonGalleryComboBoxDisplayMember;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\FormMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Ribbon.RibbonButton ribbonButtonTrackers;
        
        #line default
        #line hidden
        
        
        #line 123 "..\..\FormMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Ribbon.RibbonButton ribbonButtonTrackersData;
        
        #line default
        #line hidden
        
        
        #line 134 "..\..\FormMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridTrackers;
        
        #line default
        #line hidden
        
        
        #line 153 "..\..\FormMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboBoxCollection;
        
        #line default
        #line hidden
        
        
        #line 161 "..\..\FormMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBoxSearchTrackerData;
        
        #line default
        #line hidden
        
        
        #line 163 "..\..\FormMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Controls.UserControls.ListViewTrackers listViewTrackers;
        
        #line default
        #line hidden
        
        
        #line 176 "..\..\FormMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.WebBrowser webBrowserMap;
        
        #line default
        #line hidden
        
        
        #line 177 "..\..\FormMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridTrackersData;
        
        #line default
        #line hidden
        
        
        #line 184 "..\..\FormMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listViewTrackersData;
        
        #line default
        #line hidden
        
        
        #line 250 "..\..\FormMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label threadCount;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TqatProTrackingTool;component/formmain.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\FormMain.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 20 "..\..\FormMain.xaml"
            ((TqatProTrackingTool.FormMain)(target)).Loaded += new System.Windows.RoutedEventHandler(this.MetroWindow_Loaded);
            
            #line default
            #line hidden
            
            #line 29 "..\..\FormMain.xaml"
            ((TqatProTrackingTool.FormMain)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.MetroWindow_Closing);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 51 "..\..\FormMain.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.formCommandTrackers_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 70 "..\..\FormMain.xaml"
            ((System.Windows.Controls.Ribbon.RibbonApplicationMenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.RibbonApplicationMenuItemAts_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.labelCompany = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.labelUser = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.labelDatabaseHost = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            
            #line 93 "..\..\FormMain.xaml"
            ((System.Windows.Controls.Ribbon.RibbonApplicationMenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.RibbonApplicationMenuItemLogout_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 94 "..\..\FormMain.xaml"
            ((System.Windows.Controls.Ribbon.RibbonApplicationMenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.RibbonApplicationMenuItemExit_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.ribbonMenuButtonUser = ((System.Windows.Controls.Ribbon.RibbonMenuButton)(target));
            return;
            case 12:
            this.ribbonMenuButtonDisplayMember = ((System.Windows.Controls.Ribbon.RibbonMenuButton)(target));
            return;
            case 13:
            this.ribbonGalleryComboBoxDisplayMember = ((System.Windows.Controls.Ribbon.RibbonGallery)(target));
            
            #line 107 "..\..\FormMain.xaml"
            this.ribbonGalleryComboBoxDisplayMember.SelectionChanged += new System.Windows.RoutedPropertyChangedEventHandler<object>(this.RibbonComboBoxDisplayMember_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 14:
            this.ribbonButtonTrackers = ((System.Windows.Controls.Ribbon.RibbonButton)(target));
            
            #line 121 "..\..\FormMain.xaml"
            this.ribbonButtonTrackers.Click += new System.Windows.RoutedEventHandler(this.ribbonButtonTrackers_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            this.ribbonButtonTrackersData = ((System.Windows.Controls.Ribbon.RibbonButton)(target));
            
            #line 123 "..\..\FormMain.xaml"
            this.ribbonButtonTrackersData.Click += new System.Windows.RoutedEventHandler(this.ribbonButtonTrackersData_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.gridTrackers = ((System.Windows.Controls.Grid)(target));
            return;
            case 17:
            this.comboBoxCollection = ((System.Windows.Controls.ComboBox)(target));
            
            #line 153 "..\..\FormMain.xaml"
            this.comboBoxCollection.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.comboBoxCollection_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 18:
            this.textBoxSearchTrackerData = ((System.Windows.Controls.TextBox)(target));
            
            #line 161 "..\..\FormMain.xaml"
            this.textBoxSearchTrackerData.KeyUp += new System.Windows.Input.KeyEventHandler(this.textBoxSearchTrackerData_KeyUp);
            
            #line default
            #line hidden
            return;
            case 19:
            this.listViewTrackers = ((Controls.UserControls.ListViewTrackers)(target));
            return;
            case 20:
            this.webBrowserMap = ((System.Windows.Controls.WebBrowser)(target));
            return;
            case 21:
            this.gridTrackersData = ((System.Windows.Controls.Grid)(target));
            return;
            case 22:
            this.listViewTrackersData = ((System.Windows.Controls.ListView)(target));
            
            #line 188 "..\..\FormMain.xaml"
            this.listViewTrackersData.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.listViewTrackersData_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 190 "..\..\FormMain.xaml"
            this.listViewTrackersData.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new System.Windows.RoutedEventHandler(this.listViewTrackersData_Click));
            
            #line default
            #line hidden
            return;
            case 23:
            this.threadCount = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 2:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.MenuItem.ClickEvent;
            
            #line 39 "..\..\FormMain.xaml"
            eventSetter.Handler = new System.Windows.RoutedEventHandler(this.StyleRibbonMenuButtonUser_Click);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            case 3:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.MenuItem.ClickEvent;
            
            #line 43 "..\..\FormMain.xaml"
            eventSetter.Handler = new System.Windows.RoutedEventHandler(this.StyleRibbonMenuButtonUser_Click);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}

