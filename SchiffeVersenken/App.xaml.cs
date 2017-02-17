using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using SchiffeVersenken.Models;
using SchiffeVersenken.Services;
using SchiffeVersenken.ViewModels;
using SchiffeVersenken.Views;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace SchiffeVersenken
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        private void RegisterNavigationService()
        {
            NavigationService navigationService = new NavigationService();

            navigationService.Configure(Constants.Constants.SplashScreenPage, typeof(SplashScreenPage));
            navigationService.Configure(Constants.Constants.MainPage, typeof(MainPage));
            navigationService.Configure(Constants.Constants.SettingsPage, typeof(SettingsPage));
            navigationService.Configure(Constants.Constants.WarPage, typeof(WarPage));
            navigationService.Configure(Constants.Constants.GamePage, typeof(GamePage));
            navigationService.Configure(Constants.Constants.HighscorePage, typeof(HighscorePage));


            SimpleIoc.Default.Register<INavigationService>(() => navigationService);
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IStorageService, WinRtStorageService>();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = false;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                RegisterNavigationService();
                LoadUser();

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(SplashScreenPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
      
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        ///  Asynchron methode to load the user and highscores
        /// </summary>
        private async void LoadUser()
        {
            IStorageService storageService = ServiceLocator.Current.GetInstance<IStorageService>();
            try
            {
                ObservableCollection<UserList> userData = await storageService.Read<ObservableCollection<UserList>>("User.txt");
                if (userData != null) 
                {
                    MainView.Users = userData;
                }
                ObservableCollection<Highscores> highscoreData = await storageService.Read<ObservableCollection<Highscores>>("Highscores.txt");
                if (highscoreData != null)
                {
                    HighscoreViewModel.Highscore = highscoreData;
                }
            }
            catch (FileNotFoundException)
            {

            }    
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            SaveUser();

            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// <summary>
        ///  Asynchron methode to save the users and highscores.
        /// </summary>
        private async void SaveUser()
        {
            IStorageService storageService = ServiceLocator.Current.GetInstance<IStorageService>();
            await storageService.Write("User.txt", MainView.Users);
            await storageService.Write("Highscores.txt", App.HighscoreViewModel.Highscore);
        }

        public static MainViewModel MainView
        {
            get { return ((ViewModelLocator)Current.Resources["ViewModelLocator"]).MainViewModel; }
        }

        public static WarViewModel WarViewModel
        {
            get { return ((ViewModelLocator)Current.Resources["ViewModelLocator"]).WarViewModel; }
        }

        public static GameViewModel GameViewModel
        {
            get { return ((ViewModelLocator)Current.Resources["ViewModelLocator"]).GameViewModel; }
        }
        public static SettingsViewModel SettingsViewModel
        {
            get { return ((ViewModelLocator)Current.Resources["ViewModelLocator"]).SettingsViewModel; }
        }

        public static HighscoreViewModel HighscoreViewModel
        {
            get { return ((ViewModelLocator)Current.Resources["ViewModelLocator"]).HighscoreViewModel; }
        }
    }
}