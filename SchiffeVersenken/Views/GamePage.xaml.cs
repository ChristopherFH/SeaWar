// file:	Views\GamePage.xaml.cs
//
// summary:	Implements the game page.xaml class

using SchiffeVersenken.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Media;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SchiffeVersenken.ExtensionMethods;
using SchiffeVersenken.Models;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace SchiffeVersenken.Views
{
    /// <summary>   This page displays the both fields. It is also the page where the gameplay happens.</summary>
    ///
    /// <remarks>   Christopher, 6/19/2015. </remarks>

    public sealed partial class GamePage : Page
    {
        /// <summary>   The navigation helper. </summary>
        private NavigationHelper navigationHelper;
        /// <summary>   The default view model. </summary>
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>

        public GamePage()
        {
            MyOccurredNum = new List<int>();
            EnemyOccurredNum = new List<int>();
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
            stopWatch = new Stopwatch();
            stopWatch.Start();
            CreateField();
            if (App.SettingsViewModel.DifficultyEasy)
            {
                LoadEasyKi();
            }
            else if (App.SettingsViewModel.DifficultyModerate)
            {
                LoadModerateKi();
            }
            else if (App.SettingsViewModel.DifficultyHard)
            {
                LoadHardKi();
            }

            App.GameViewModel.Moves = 0;
            App.GameViewModel.GameOver = false;
            EnemyOccurredNum.Clear();
            MyOccurredNum.Clear();

            this.navigationHelper.OnNavigatedTo(e);
        }

        /// <summary>   The stop watch. </summary>
        private Stopwatch stopWatch;

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

        /// <summary>   stores the occurred numbers to prevent from double pressing a button. </summary>
        private List<int> MyOccurredNum;
        /// <summary>   The enemy occurred number. </summary>
        private List<int> EnemyOccurredNum;

        /// <summary>   Asynchron methode which represents the easy enemy. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>

        private async void LoadEasyKi()
        {
            if (App.SettingsViewModel.DifficultyEasy)
            {
                while (App.GameViewModel.GameOver == false)
                {
                    if (!App.SettingsViewModel.DifficultyEasy)
                    {
                        return;
                    }
                    await Task.Delay(1);
                    while (App.GameViewModel.WhosTurn == false)
                    {
                        Random random = new Random();
                        int i = random.Next(0, 100);
                        var button = this.ContentRoot.Children.OfType<Button>().Single(x => x.TabIndex == i);
                        if (!MyOccurredNum.Contains(i))
                        {
                            await Task.Delay(1000);
                            MyOccurredNum.Add(i);
                            if (App.GameViewModel.MyField[i])
                            {
                                if (button != null)
                                {
                                    button.Background = new SolidColorBrush(Colors.Red);
                                    App.GameViewModel.MyField[i] = false;
                                }
                            }
                            else
                            {
                                if (button != null)
                                {
                                    if (App.SettingsViewModel.Soundeffect)
                                    {
                                        sound.Play();
                                    }
                                    button.Background = new SolidColorBrush(Colors.Blue);
                                    App.GameViewModel.MyField[i] = false;
                                }
                            }
                            App.GameViewModel.WhosTurn = true;
                            App.GameViewModel.Arrow = 180;
                        }
                    }
                    //   App.GameViewModel.MyField.SetAllValues(false);

                    //interrupt async methode
                    var frame = Window.Current.Content as Frame;
                    if (frame != null)
                    {
                        if (frame.Content != null)
                        {
                            var pageType = frame.Content.GetType();
                            if (pageType.FullName.Equals("SchiffeVersenken.Views.MainPage"))
                            {
                                return;
                            }
                        }
                    }
                    if ((App.GameViewModel.MyField.CheckAllValues(false)) || (App.GameViewModel.EnemyField.CheckAllValues(false)))
                    {
                        App.GameViewModel.GameOver = true;
                        string winner = "Congratulations! You win";
                        string loser = "Sorry! You loose";

                        // Create the message dialog and set its content
                        MessageDialog messageDialog;

                        if ((App.GameViewModel.MyField.CheckAllValues(false)))
                        {

                            messageDialog = new MessageDialog(loser);
                        }
                        else
                        {
                            stopWatch.Stop();
                            TimeSpan ts = stopWatch.Elapsed;
                            // Format and display the TimeSpan value.
                            string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                                 ts.Minutes, ts.Seconds,
                                ts.Milliseconds / 10);
                            messageDialog = new MessageDialog(winner);
                            App.HighscoreViewModel.Highscore.Add(new Highscores(App.MainView.Username, "Easy", App.GameViewModel.Moves, elapsedTime));
                        }


                        // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
                        messageDialog.Commands.Add(new UICommand(
                            "OK",
                            new UICommandInvokedHandler(this.CommandInvokedHandler)));

                        // Set the command that will be invoked by default
                        messageDialog.DefaultCommandIndex = 0;

                        // Set the command to be invoked when escape is pressed
                        messageDialog.CancelCommandIndex = 1;


                        // Show the message dialog
                        await messageDialog.ShowAsync();
                    }
                }
            }
        }

        /// <summary>   Asynchron methode which represents the moderate enemy. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>

        private async void LoadModerateKi()
        {
            if (App.SettingsViewModel.DifficultyModerate)
            {
                int i = GetFirstField();
                while (App.GameViewModel.GameOver == false)
                {
                    if (!App.SettingsViewModel.DifficultyModerate)
                    {
                        return;
                    }
                    await Task.Delay(1);
                    while (App.GameViewModel.WhosTurn == false)
                    {

                        var button = this.ContentRoot.Children.OfType<Button>().Single(x => x.TabIndex == i);
                        if (!MyOccurredNum.Contains(i))
                        {
                            await Task.Delay(1000);
                            MyOccurredNum.Add(i);
                            if (App.GameViewModel.MyField[i])
                            {
                                if (button != null)
                                {
                                    button.Background = new SolidColorBrush(Colors.Red);
                                    App.GameViewModel.MyField[i] = false;
                                }
                                i = GetNextField(i, App.GameViewModel.MyField);
                                if (i == int.MinValue)
                                {
                                    i = GetFirstField();
                                }
                            }
                            else
                            {
                                if (button != null)
                                {
                                    if (App.SettingsViewModel.Soundeffect)
                                    {
                                        sound.Play();
                                    }
                                    button.Background = new SolidColorBrush(Colors.Blue);
                                    App.GameViewModel.MyField[i] = false;
                                }
                            }
                            App.GameViewModel.WhosTurn = true;
                            App.GameViewModel.Arrow = 180;
                        }
                        while (MyOccurredNum.Contains(i))
                        {
                            i = GetFirstField();
                        }
                    }
                    //interrupt async methode
                    var frame = Window.Current.Content as Frame;
                    if (frame != null)
                    {
                        if (frame.Content != null)
                        {
                            var pageType = frame.Content.GetType();
                            if (pageType.FullName.Equals("SchiffeVersenken.Views.MainPage"))
                            {
                                return;
                            }
                        }
                    }
                    //   App.GameViewModel.MyField.SetAllValues(false);
                    if ((App.GameViewModel.MyField.CheckAllValues(false)) || (App.GameViewModel.EnemyField.CheckAllValues(false)))
                    {
                        App.GameViewModel.GameOver = true;
                        string winner = "Congratulations! You win";
                        string loser = "Sorry! You loose";

                        // Create the message dialog and set its content
                        MessageDialog messageDialog;

                        if ((App.GameViewModel.MyField.CheckAllValues(false)))
                        {
                            messageDialog = new MessageDialog(loser);
                        }
                        else
                        {
                            stopWatch.Stop();
                            TimeSpan ts = stopWatch.Elapsed;
                            // Format and display the TimeSpan value.
                            string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                                 ts.Minutes, ts.Seconds,
                                ts.Milliseconds / 10);
                            messageDialog = new MessageDialog(winner);
                            App.HighscoreViewModel.Highscore.Add(new Highscores(App.MainView.Username, "Moderate", App.GameViewModel.Moves, elapsedTime));
                        }


                        // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
                        messageDialog.Commands.Add(new UICommand(
                            "OK",
                            new UICommandInvokedHandler(this.CommandInvokedHandler)));

                        // Set the command that will be invoked by default
                        messageDialog.DefaultCommandIndex = 0;

                        // Set the command to be invoked when escape is pressed
                        messageDialog.CancelCommandIndex = 1;

                        // Show the message dialog
                        await messageDialog.ShowAsync();
                    }
                }
            }
        }

        /// <summary>   Asynchron methode which represents the hard enemy. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>

        private async void LoadHardKi()
        {
            if (App.SettingsViewModel.DifficultyHard)
            {
                int i = GetFirstField();
                while (App.GameViewModel.GameOver == false)
                {
                    if (!App.SettingsViewModel.DifficultyHard)
                    {
                        return;
                    }
                    await Task.Delay(1);
                    while (App.GameViewModel.WhosTurn == false)
                    {

                        var button = this.ContentRoot.Children.OfType<Button>().Single(x => x.TabIndex == i);
                        if (!MyOccurredNum.Contains(i))
                        {
                            await Task.Delay(1000);
                            MyOccurredNum.Add(i);
                            if (App.GameViewModel.MyField[i])
                            {
                                if (button != null)
                                {
                                    button.Background = new SolidColorBrush(Colors.Red);
                                    App.GameViewModel.MyField[i] = false;
                                }
                                i = GetNextField(i, App.GameViewModel.MyField);
                                if (i == int.MinValue)
                                {
                                    i = GetNextShip(App.GameViewModel.MyField);

                                }
                            }
                            else
                            {
                                if (button != null)
                                {
                                    if (App.SettingsViewModel.Soundeffect)
                                    {
                                        sound.Play();
                                    }
                                    button.Background = new SolidColorBrush(Colors.Blue);
                                    App.GameViewModel.MyField[i] = false;
                                }
                            }
                            App.GameViewModel.WhosTurn = true;
                            App.GameViewModel.Arrow = 180;
                        }
                        while (MyOccurredNum.Contains(i))
                        {
                            i = GetFirstField();
                        }
                    }
                    //interrupt async methode
                    var frame = Window.Current.Content as Frame;
                    if (frame != null)
                    {
                        if (frame.Content != null)
                        {
                            var pageType = frame.Content.GetType();
                            if (pageType.FullName.Equals("SchiffeVersenken.Views.MainPage"))
                            {
                                return;
                            }
                        }
                    }
                    //   App.GameViewModel.MyField.SetAllValues(false);
                    if ((App.GameViewModel.MyField.CheckAllValues(false)) || (App.GameViewModel.EnemyField.CheckAllValues(false)))
                    {
                        App.GameViewModel.GameOver = true;
                        string winner = "Congratulations! You win";
                        string loser = "Sorry! You loose";

                        // Create the message dialog and set its content
                        MessageDialog messageDialog;

                        if ((App.GameViewModel.MyField.CheckAllValues(false)))
                        {
                            messageDialog = new MessageDialog(loser);
                        }
                        else
                        {
                            stopWatch.Stop();
                            TimeSpan ts = stopWatch.Elapsed;
                            // Format and display the TimeSpan value.
                            string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                                 ts.Minutes, ts.Seconds,
                                ts.Milliseconds / 10);
                            messageDialog = new MessageDialog(winner);
                            App.HighscoreViewModel.Highscore.Add(new Highscores(App.MainView.Username, "Hard", App.GameViewModel.Moves, elapsedTime));
                        }


                        // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
                        messageDialog.Commands.Add(new UICommand(
                            "OK",
                            new UICommandInvokedHandler(this.CommandInvokedHandler)));

                        // Set the command that will be invoked by default
                        messageDialog.DefaultCommandIndex = 0;

                        // Set the command to be invoked when escape is pressed
                        messageDialog.CancelCommandIndex = 1;

                        // Show the message dialog
                        await messageDialog.ShowAsync();
                    }
                }
            }
        }

        /// <summary>   Gets a random position. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <returns>   The first field. </returns>

        public int GetFirstField()
        {
            Random rnd = new Random();
            int random = rnd.Next(0, 100);
            return random;
        }

        /// <summary>   Gets the nerby field if a ship has been hit. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="index">    Zero-based index of the. </param>
        /// <param name="field">    The field. </param>
        ///
        /// <returns>   The next field. </returns>

        public int GetNextField(int index, bool[] field)
        {
            if (index == 0)
            {
                if (field[index + 1])
                {
                    return index + 1;
                }
                return index + 10;
            }
            if (index > 0 && index < 10)
            {
                if (field[index + 1])
                {
                    return index + 1;
                }
                if (field[index + 10])
                {
                    return index + 10;
                }
                if (field[index - 1])
                {
                    return index - 1;
                }
            }
            else if (index <= 99 && index > 89)
            {
                if (field[index - 1])
                {
                    return index - 1;
                }
                return index - 10;
            }
            else
            {
                if (field[index + 1])
                {
                    return index + 1;
                }
                if (field[index - 1])
                {
                    return index - 1;
                }
                if (field[index - 10])
                {
                    return index - 10;
                }
                if (field[index + 10])
                {
                    return index + 10;
                }
            }
            return int.MinValue;
        }

        /// <summary>   Gets the field index of the next ship. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="field">    The field. </param>
        ///
        /// <returns>   The next ship. </returns>

        public int GetNextShip(bool[] field)
        {
            for (int i = 0; i < 100; i++)
            {
                if (field[i])
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>   Navigates to the HighscorePage. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="command">  The command. </param>

        private void CommandInvokedHandler(IUICommand command)
        {
            Frame.Navigate(typeof(HighscorePage));
        }

        /// <summary>   Draws the ships on the own field. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>

        private void CreateField()
        {
            UpdateTime();

            //MyField
            for (int i = 0; i < 100; i++)
            {
                if (App.GameViewModel.MyField[i])
                {
                    var button =
                    this.ContentRoot.Children.OfType<Button>()
                        .Single(x => x.TabIndex == i);
                    button.Background = new SolidColorBrush(Colors.Black);
                }
            }

            //  EnemyField
            //for (int i = 0; i < 100; i++)
            //{
            //    if (App.GameViewModel.EnemyField[i])
            //    {
            //        var button =
            //        this.ContentRoot2.Children.OfType<Button>()
            //            .Single(x => x.TabIndex == i);
            //        button.Background = new SolidColorBrush(Colors.Black);
            //    }
            //}
        }

        /// <summary>   Asynchrone methode wich updates the time on the Gamepage. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>

        private async void UpdateTime()
        {
            App.GameViewModel.DisplyTime.Start();
            App.GameViewModel.DisplyTime.Restart();
            while (App.GameViewModel.GameOver == false)
            {
                TimeSpan ts = App.GameViewModel.DisplyTime.Elapsed;

                // Format and display the TimeSpan value.     
                App.GameViewModel.Time = String.Format("{0:00}:{1:00}.{2:00}",
                     ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                await Task.Delay(1);
            }
            App.GameViewModel.DisplyTime.Stop();
        }

        /// <summary>   Reacts on the user imput and calls the ColorField methode if valid. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="sender">   The source of the event; typically <see cref="NavigationHelper"/> </param>
        /// <param name="e">        Routed event information. </param>

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (App.GameViewModel.GameOver == false)
            {
                if (App.GameViewModel.WhosTurn)
                {
                    var button = sender as Button;
                    int index = 0;
                    if (button != null)
                    {
                        index = button.TabIndex;
                    }

                    if (!EnemyOccurredNum.Contains(index))
                    {
                        EnemyOccurredNum.Add(index);
                        ColorField(button, index);
                    }
                }
            }
        }

        /// <summary>   Colors the fiven field. Red if a ship has been hit or blue if not. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="button">   The button control. </param>
        /// <param name="index">    Zero-based index of the. </param>

        private void ColorField(Button button, int index)
        {
            if (ContentRoot2.Children.Contains(button))
            {
                if (App.GameViewModel.EnemyField[index])
                {
                    if (button != null)
                    {
                        button.Background = new SolidColorBrush(Colors.Red);
                        App.GameViewModel.EnemyField[index] = false;
                    }
                }
                else
                {
                    if (button != null)
                    {
                        if (App.SettingsViewModel.Soundeffect)
                        {
                            sound.Play();
                        }
                        button.Background = new SolidColorBrush(Colors.Blue);
                        App.GameViewModel.EnemyField[index] = false;
                    }
                }
                App.GameViewModel.Arrow = 0;
                App.GameViewModel.WhosTurn = false;
                App.GameViewModel.Moves++;
            }
        }
    }
}
