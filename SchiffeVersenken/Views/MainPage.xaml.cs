// file:	Views\MainPage.xaml.cs
//
// summary:	Implements the main page.xaml class

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
using Windows.UI;
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
    /// <summary>   This page represents the entry of the app. </summary>
    ///
    /// <remarks>   Christopher, 6/19/2015. </remarks>

    public sealed partial class MainPage : Page
    {
        /// <summary>   The navigation helper. </summary>
        private NavigationHelper navigationHelper;
        /// <summary>   The default view model. </summary>
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>

        public MainPage()
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
            ResetField();

#pragma warning disable 4014
            PlaceEnemy();
#pragma warning restore 4014
            this.navigationHelper.OnNavigatedTo(e);

            // checks if stopwatch is stil running
            if (App.GameViewModel.DisplyTime.IsRunning)
            {
                 App.GameViewModel.DisplyTime.Stop();
            }
        }

        /// <summary>   Resets the whole field and the accompanying properties. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>

        private void ResetField()
        {
            App.GameViewModel.WhichShip.SetAllValues(false);
            App.GameViewModel.EnemyField.SetAllValues(false);
            App.GameViewModel.WhichShip[0] = true;
            App.GameViewModel.AllShipsSet = false;
            App.GameViewModel.ShipLength = 5;
        }

 
        /// <summary>   The ships. </summary>
        Ships ships = new Ships();

        /// <summary>   Sets a ship in the desired position. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="index">        Zero-based index of the. </param>
        /// <param name="shipLength">   Length of the ship. </param>
        /// <param name="direction">    true to direction. </param>

        public void PlaceShip(int index, int shipLength, bool direction)
        {
            bool shipSet = false;
            try
            {
                if (direction)
                {
                    if (FieldIsValidHorizontal(index, shipLength))
                    {
                        App.GameViewModel.Index = index;
                        for (int i = 0; i < shipLength; i++)
                        {
                            App.GameViewModel.EnemyField[index] = true;
                            index--;
                            shipSet = true;
                        }
                    }
                }
                else
                {
                    if (FieldIsValidVertical(index, shipLength))
                    {
                        App.GameViewModel.Index = index;
                        for (int i = 0; i < shipLength; i++)
                        {
                            App.GameViewModel.EnemyField[index] = true;
                            index -= 10;
                            shipSet = true;
                        }
                    }
                }
                if (shipSet)
                {
                    ships.ShipSet(App.GameViewModel.WhichShip);
                }
                if (ships.AllShipsPlaced)
                {
                    App.GameViewModel.AllShipsSet = true;
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Checks if it is possible to place the chip on this point in vertical direction.
        /// </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="index">        Zero-based index of the. </param>
        /// <param name="shipLength">   Length of the ship. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>

        private bool FieldIsValidVertical(int index, int shipLength)
        {
            bool isValid = false;
            if (!(ships.IsSet(App.GameViewModel.WhichShip)))
            {
                if (!(App.GameViewModel.EnemyField[index]))
                {
                    if (index - 10 * (shipLength - 1) >= 0)
                    {
                        if (index % 10 == 9 && index != 99)
                        {
                            for (int i = 0; i < shipLength; i++)
                            {

                                if (index != 9)
                                {
                                    if (App.GameViewModel.EnemyField[index - 1] != true &&
                                        App.GameViewModel.EnemyField[index + 9] != true &&
                                        App.GameViewModel.EnemyField[index - 11] != true &&
                                        App.GameViewModel.EnemyField[index + 10] != true &&
                                        App.GameViewModel.EnemyField[index - 10] != true)
                                    {
                                        index -= 10;
                                        isValid = true;
                                    }
                                    else
                                    {
                                        isValid = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (App.GameViewModel.EnemyField[index - 1] != true &&
                                         App.GameViewModel.EnemyField[index + 9] != true &&
                                         App.GameViewModel.EnemyField[index + 10] != true)
                                    {
                                        index -= 10;
                                        isValid = true;
                                    }
                                    else
                                    {
                                        isValid = false;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (index == 99)
                        {
                            for (int i = 0; i < shipLength; i++)
                            {
                                if (App.GameViewModel.EnemyField[index - 1] != true &&
                                    App.GameViewModel.EnemyField[index - 11] != true &&
                                    App.GameViewModel.EnemyField[index - 10] != true)
                                {
                                    index -= 10;
                                    isValid = true;
                                }
                                else
                                {
                                    isValid = false;
                                    break;
                                }
                            }
                        }
                        else if (index > 89 && index < 99)
                        {
                            for (int i = 0; i < shipLength; i++)
                            {
                                if (App.GameViewModel.EnemyField[index - 1] != true &&
                                    App.GameViewModel.EnemyField[index + 1] != true &&
                                    App.GameViewModel.EnemyField[index - 11] != true &&
                                    App.GameViewModel.EnemyField[index - 10] != true &&
                                    App.GameViewModel.EnemyField[index - 9] != true)
                                {
                                    index -= 10;
                                    isValid = true;
                                }
                                else
                                {
                                    isValid = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < shipLength; i++)
                            {
                                if (index > 10)
                                {
                                    if (App.GameViewModel.EnemyField[index - 1] != true &&
                                        App.GameViewModel.EnemyField[index + 9] != true &&
                                        App.GameViewModel.EnemyField[index - 9] != true &&
                                        App.GameViewModel.EnemyField[index + 1] != true &&
                                        App.GameViewModel.EnemyField[index + 10] != true &&
                                        App.GameViewModel.EnemyField[index + 11] != true &&
                                        App.GameViewModel.EnemyField[index - 10] != true &&
                                        App.GameViewModel.EnemyField[index - 11] != true)
                                    {
                                        index -= 10;
                                        isValid = true;
                                    }
                                    else
                                    {
                                        isValid = false;
                                        break;
                                    }
                                }
                                else if (index == 10)
                                {
                                    if (App.GameViewModel.EnemyField[index + 1] != true &&
                                       App.GameViewModel.EnemyField[index + 9] != true &&
                                       App.GameViewModel.EnemyField[index - 9] != true &&
                                        App.GameViewModel.EnemyField[index + 11] != true &&
                                       App.GameViewModel.EnemyField[index + 10] != true)
                                    {
                                        index -= 10;
                                        isValid = true;
                                    }
                                    else
                                    {
                                        isValid = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (App.GameViewModel.EnemyField[index + 1] != true &&
                                        App.GameViewModel.EnemyField[index + 9] != true &&
                                        App.GameViewModel.EnemyField[index + 10] != true)
                                    {
                                        index -= 10;
                                        isValid = true;
                                    }
                                    else
                                    {
                                        isValid = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return isValid;
        }

        /// <summary>
        /// Checks if it is possible to place the chip on this point in horizontal direction.
        /// </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="index">        Zero-based index of the. </param>
        /// <param name="shipLength">   Length of the ship. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>

        private bool FieldIsValidHorizontal(int index, int shipLength)
        {
            bool isValid = false;
            if (!(ships.IsSet(App.GameViewModel.WhichShip)))
            {
                if (!(App.GameViewModel.EnemyField[index]))
                {
                    if ((index % 10 == 1 && shipLength == 2) || (index % 10 == 2 && shipLength <= 3) || (index % 10 == 3 && shipLength <= 4) || (index % 10 == 4 && shipLength <= 5) || (index % 10 > 4))
                    {
                        if (index == 99)
                        {
                            for (int i = 0; i < shipLength; i++)
                            {
                                if (App.GameViewModel.EnemyField[index - 10] != true &&
                                     App.GameViewModel.EnemyField[index - 1] != true
                                     && App.GameViewModel.EnemyField[index - 11] != true)
                                {
                                    isValid = true;
                                    index--;
                                }
                                else
                                {
                                    isValid = false;
                                    break;
                                }
                            }
                        }
                        else if (index < 99 && index >= 89)
                        {
                            if (index == 89)
                            {
                                for (int i = 0; i < shipLength; i++)
                                {
                                    if (App.GameViewModel.EnemyField[index - 10] != true &&
                                        App.GameViewModel.EnemyField[index + 10] != true &&
                                        App.GameViewModel.EnemyField[index - 1] != true &&
                                        App.GameViewModel.EnemyField[index - 11] != true)
                                    {
                                        index--;
                                        isValid = true;
                                    }
                                    else
                                    {
                                        isValid = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < shipLength; i++)
                                {
                                    if (App.GameViewModel.EnemyField[index + 1] != true &&
                                        App.GameViewModel.EnemyField[index - 1] != true &&
                                        App.GameViewModel.EnemyField[index - 10] != true &&
                                        App.GameViewModel.EnemyField[index - 9] != true &&
                                        App.GameViewModel.EnemyField[index - 11] != true)
                                    {
                                        index--;
                                        isValid = true;
                                    }
                                    else
                                    {
                                        isValid = false;
                                        break;
                                    }
                                }
                            }

                        }
                        else if (index > 0 && index <= 9 && !(index + 1 - shipLength < 0))
                        {
                            for (int i = 0; i < shipLength; i++)
                            {

                                if (index != 0)
                                {
                                    if (index == 9)
                                    {
                                        if (App.GameViewModel.EnemyField[index - 1] != true &&
                                            App.GameViewModel.EnemyField[index - 9] != true &&
                                            App.GameViewModel.EnemyField[index + 9] != true &&
                                            App.GameViewModel.EnemyField[index + 11] != true)
                                        {
                                            isValid = true;
                                            index--;
                                        }
                                        else
                                        {
                                            isValid = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (App.GameViewModel.EnemyField[index + 1] != true &&
                                        App.GameViewModel.EnemyField[index - 1] != true &&
                                        App.GameViewModel.EnemyField[index + 10] != true &&
                                        App.GameViewModel.EnemyField[index + 11] != true &&
                                        App.GameViewModel.EnemyField[index + 9] != true)
                                        {
                                            isValid = true;
                                            index--;
                                        }
                                        else
                                        {
                                            isValid = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (App.GameViewModel.EnemyField[index + 1] != true &&
                                    App.GameViewModel.EnemyField[index + 10] != true &&
                                    App.GameViewModel.EnemyField[index + 9] != true)
                                    {
                                        isValid = true;
                                        index--;
                                    }
                                    else
                                    {
                                        isValid = false;
                                        break;
                                    }
                                }
                            }

                        }
                        else if (index % 10 == 9)
                        {
                            for (int i = 0; i < shipLength; i++)
                            {


                                if (App.GameViewModel.EnemyField[index - 1] != true &&
                                App.GameViewModel.EnemyField[index + 9] != true &&
                                App.GameViewModel.EnemyField[index - 10] != true &&
                                App.GameViewModel.EnemyField[index - 11] != true &&
                                App.GameViewModel.EnemyField[index + 10] != true)
                                {
                                    isValid = true;
                                    index--;
                                }
                                else
                                {
                                    isValid = false;
                                    break;
                                }
                            }

                        }
                        else
                        {
                            for (int i = 0; i < shipLength; i++)
                            {
                                if (index % 10 == 0)
                                {
                                    if (App.GameViewModel.EnemyField[index + 1] != true &&
                                        App.GameViewModel.EnemyField[index - 9] != true &&
                                        App.GameViewModel.EnemyField[index + 9] != true &&
                                        App.GameViewModel.EnemyField[index + 11] != true &&
                                         App.GameViewModel.EnemyField[index - 10] != true)
                                    {
                                        isValid = true;
                                        index--;
                                    }
                                    else
                                    {
                                        isValid = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (App.GameViewModel.EnemyField[index - 1] != true &&
                                    App.GameViewModel.EnemyField[index + 1] != true &&
                                    App.GameViewModel.EnemyField[index - 9] != true &&
                                    App.GameViewModel.EnemyField[index + 9] != true &&
                                    App.GameViewModel.EnemyField[index - 11] != true &&
                                    App.GameViewModel.EnemyField[index + 11] != true)
                                    {
                                        isValid = true;
                                        index--;
                                    }
                                    else
                                    {
                                        isValid = false;
                                        break;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return isValid;
        }

        /// <summary>   Places all ships for the enemy in random psoitions. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <returns>   A Task&lt;bool&gt; </returns>

        private async Task<bool> PlaceEnemy()
        {
            int counter = 0;
            ships.ResetValues();

            while (App.GameViewModel.AllShipsSet == false)
            {
                Random random = new Random();
                var randomNumber = random.Next(0, 100);
                counter++;
                PlaceShip(randomNumber, App.GameViewModel.ShipLength, GetRandomBoolean());
                if (ships.Battleship == 0)
                {
                    App.GameViewModel.WhichShip.SetAllValues(false);
                    App.GameViewModel.WhichShip[1] = true;
                    App.GameViewModel.ShipLength = 4;
                }
                if (ships.Cruiser == 0)
                {
                    App.GameViewModel.WhichShip.SetAllValues(false);
                    App.GameViewModel.WhichShip[2] = true;
                    App.GameViewModel.ShipLength = 3;
                }
                if (ships.Destroyer == 0)
                {
                    App.GameViewModel.WhichShip.SetAllValues(false);
                    App.GameViewModel.WhichShip[3] = true;
                    App.GameViewModel.ShipLength = 2;
                }
                if (counter > 200)
                {
                    ResetField();
                    await PlaceEnemy();
                }
                await Task.Delay(1);
            }
            return true;
        }



        /// <summary>   true to change. </summary>
        private bool change = true;

        /// <summary>   Returns a random boolean. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>

        public bool GetRandomBoolean()
        {
            if (change)
            {
                change = false;
                return ships.Battleship != 0 || ships.Cruiser != 0;
            }
            else
            {
                change = true;
                return ships.Battleship == 0 && ships.Cruiser == 0;
            }
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

       
    }
}
