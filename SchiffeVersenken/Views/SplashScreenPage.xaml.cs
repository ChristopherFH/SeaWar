// file:	Views\SplashScreenPage.xaml.cs
//
// summary:	Implements the splash screen page.xaml class

using SchiffeVersenken.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using SchiffeVersenken.ExtensionMethods;
using SchiffeVersenken.Models;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace SchiffeVersenken.Views
{
    /// <summary>  This page is shown directly after the start of the app.  </summary>
    ///
    /// <remarks>   Christopher, 6/19/2015. </remarks>

    public sealed partial class SplashScreenPage : Page
    {
        /// <summary>   The navigation helper. </summary>
        private NavigationHelper navigationHelper;
        /// <summary>   The default view model. </summary>
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>

        public SplashScreenPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        ///
        /// <value> The navigation helper. </value>

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>. This can be changed to a strongly typed view
        /// model.
        /// </summary>
        ///
        /// <value> The default view model. </value>

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also provided
        /// when recreating a page from a prior session.
        /// </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="sender">   The source of the event; typically <see cref="NavigationHelper"/> </param>
        /// <param name="e">        Event data that provides both the navigation parameter passed to
        ///                         <see cref="Frame.Navigate(Type, Object)"/> when this page was
        ///                         initially requested and a dictionary of state preserved by this page
        ///                         during an earlier session.  The state will be null the first time a
        ///                         page is visited. </param>

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the page is
        /// discarded from the navigation cache.  Values must conform to the serialization requirements
        /// of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="sender">   The source of the event; typically <see cref="NavigationHelper"/> </param>
        /// <param name="e">        Event data that provides an empty dictionary to be populated with
        ///                         serializable state. </param>

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow NavigationHelper to respond to
        /// the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>. The navigation parameter is available in the
        /// LoadState method in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="e">    Provides data for navigation methods and event handlers that cannot
        ///                     cancel the navigation request. </param>

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {       
            this.navigationHelper.OnNavigatedTo(e);
        }

        /// <summary>
        /// Invoked immediately after the Page is unloaded and is no longer the current source of a
        /// parent Frame.
        /// </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="e">    Event data that can be examined by overriding code. The event data is
        ///                     representative of the navigation that has unloaded the current Page. </param>

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        /// <summary>   Event handler. Called by ContentRoot for loaded events. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="sender">   The source of the event; typically <see cref="NavigationHelper"/> </param>
        /// <param name="e">        Routed event information. </param>

        private void ContentRoot_Loaded(object sender, RoutedEventArgs e)
        {   
            Wait();
        }

        /// <summary>   Delays the forwarding. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>

        private async void Wait()
        {
            await Task.Delay(3000);
            this.Frame.Navigate(typeof(MainPage));
        }
   
    }
}
