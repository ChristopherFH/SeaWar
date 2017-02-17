// file:	Views\WarPage.xaml.cs
//
// summary:	Implements the war page.xaml class

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SchiffeVersenken.Common;
using SchiffeVersenken.ExtensionMethods;
using SchiffeVersenken.Models;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace SchiffeVersenken.Views
{
    /// <summary>   On this page the user can place the ships.  </summary>
    ///
    /// <remarks>   Christopher, 6/19/2015. </remarks>

    public sealed partial class WarPage : Page
    {
        /// <summary>   The navigation helper. </summary>
        private NavigationHelper navigationHelper;
        /// <summary>   The default view model. </summary>
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>

        public WarPage()
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
        ///                         initially requested and
        ///                         a dictionary of state preserved by this page during an earlier
        ///                         session.  The state will be null the first time a page is visited. </param>

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
        /// ### <param name="e">    Provides data for navigation methods and event handlers that cannot
        ///                         cancel the navigation request. </param>

        private Ships ships = new Ships();

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="e">    Event data that can be examined by overriding code. The event data is
        ///                     representative of the pending navigation that will load the current Page.
        ///                     Usually the most relevant property to examine is Parameter. </param>

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ResetField();

            this.navigationHelper.OnNavigatedTo(e);
        }

        #endregion

        /// <summary>   Reacts on the userinput and gets the button tabindex. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="sender">   The source of the event; typically <see cref="NavigationHelper"/> </param>
        /// <param name="e">        Routed event information. </param>

        private void ButtonClickEvent(object sender, RoutedEventArgs e)
        {
            bool direction = true;
            Button but = sender as Button;
            direction = App.WarViewModel.Direction.Equals("←");
            if (but != null) PlaceShip(but.TabIndex, App.WarViewModel.ShipLength, direction);
        }

        /// <summary>
        /// Sets a ship in the desired position. If all ships are placed the AllShipsPlaced variable
        /// becomes true. After this, it is prossible to start the game.
        /// </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="index">        Zero-based index of the field. </param>
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
                        App.WarViewModel.Index = index;
                        App.WarViewModel.PreviousShip = App.WarViewModel.ShipLength;
                        for (int i = 0; i < shipLength; i++)
                        {
                            var button =
                                this.ContentRoot.Children.OfType<Button>()
                                    .Single(x => x.TabIndex == index);
                            button.Background = new SolidColorBrush(Colors.Black);
                            App.WarViewModel.Field[index] = true;
                            index--;
                            shipSet = true;
                        }
                    }
                }
                else
                {
                    if (FieldIsValidVertical(index, shipLength))
                    {
                        App.WarViewModel.Index = index;
                        App.WarViewModel.PreviousShip = App.WarViewModel.ShipLength;
                        for (int i = 0; i < shipLength; i++)
                        {
                            var button =
                                this.ContentRoot.Children.OfType<Button>().Single(x => x.TabIndex == index);
                            button.Background = new SolidColorBrush(Colors.Black);
                            App.WarViewModel.Field[index] = true;
                            index -= 10;
                            shipSet = true;
                        }
                    }
                }
                if (shipSet)
                {
                    ships.ShipSet(App.WarViewModel.WhichShip);
                    UpdateGui(App.WarViewModel.WhichShip);
                }
                if (ships.AllShipsPlaced)
                {
                    App.GameViewModel.MyField = (bool[])App.WarViewModel.Field.Clone();
                    App.WarViewModel.AllShipsSet = true;
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void UpdateGui(bool[] ship)
        {
            if (ship[0] == true)
            {
                App.WarViewModel.Battleship--;
            }
            else if (ship[1] == true)
            {
                App.WarViewModel.Cruiser--;
            }
            else if (ship[2] == true)
            {
                App.WarViewModel.Destroyer--;
            }
            else
            {
                App.WarViewModel.Submarine--;
            }
        }

        /// <summary>
        /// Checks if it is possible to place the chip on this point in vertical direction.
        /// </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="index">        Zero-based index of the field. </param>
        /// <param name="shipLength">   Length of the ship. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>

        private bool FieldIsValidVertical(int index, int shipLength)
        {
            bool isValid = false;
            if (!(ships.IsSet(App.WarViewModel.WhichShip)))
            {
                if (!(App.WarViewModel.Field[index]))
                {
                    if (index - 10 * (shipLength - 1) >= 0)
                    {
                        if (index % 10 == 9 && index != 99)
                        {
                            for (int i = 0; i < shipLength; i++)
                            {

                                if (index != 9)
                                {
                                    if (App.WarViewModel.Field[index - 1] != true &&
                                        App.WarViewModel.Field[index + 9] != true &&
                                        App.WarViewModel.Field[index - 11] != true &&
                                        App.WarViewModel.Field[index + 10] != true &&
                                        App.WarViewModel.Field[index - 10] != true)
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
                                    if (App.WarViewModel.Field[index - 1] != true &&
                                         App.WarViewModel.Field[index + 9] != true &&
                                         App.WarViewModel.Field[index + 10] != true)
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
                                if (App.WarViewModel.Field[index - 1] != true &&
                                    App.WarViewModel.Field[index - 11] != true &&
                                    App.WarViewModel.Field[index - 10] != true)
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
                                if (App.WarViewModel.Field[index - 1] != true &&
                                    App.WarViewModel.Field[index + 1] != true &&
                                    App.WarViewModel.Field[index - 11] != true &&
                                    App.WarViewModel.Field[index - 10] != true &&
                                    App.WarViewModel.Field[index - 9] != true)
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
                                    if (App.WarViewModel.Field[index - 1] != true &&
                                        App.WarViewModel.Field[index + 9] != true &&
                                        App.WarViewModel.Field[index - 9] != true &&
                                        App.WarViewModel.Field[index + 1] != true &&
                                        App.WarViewModel.Field[index + 10] != true &&
                                        App.WarViewModel.Field[index + 11] != true &&
                                        App.WarViewModel.Field[index - 10] != true &&
                                        App.WarViewModel.Field[index - 11] != true)
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
                                    if (App.WarViewModel.Field[index + 1] != true &&
                                       App.WarViewModel.Field[index + 9] != true &&
                                       App.WarViewModel.Field[index - 9] != true &&
                                       App.WarViewModel.Field[index + 11] != true &&
                                       App.WarViewModel.Field[index + 10] != true)
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
                                    if (App.WarViewModel.Field[index + 1] != true &&
                                        App.WarViewModel.Field[index + 9] != true &&
                                        App.WarViewModel.Field[index + 10] != true)
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
            if (!(ships.IsSet(App.WarViewModel.WhichShip)))
            {
                if (!(App.WarViewModel.Field[index]))
                {
                    if ((index % 10 == 1 && shipLength == 2) || (index % 10 == 2 && shipLength <= 3) || (index % 10 == 3 && shipLength <= 4) || (index % 10 == 4 && shipLength <= 5) || (index % 10 > 4))
                    {
                        if (index == 99)
                        {
                            for (int i = 0; i < shipLength; i++)
                            {
                                if (App.WarViewModel.Field[index - 10] != true &&
                                     App.WarViewModel.Field[index - 1] != true
                                     && App.WarViewModel.Field[index - 11] != true)
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
                                    if (App.WarViewModel.Field[index - 10] != true &&
                                        App.WarViewModel.Field[index + 10] != true &&
                                        App.WarViewModel.Field[index - 1] != true &&
                                        App.WarViewModel.Field[index - 11] != true)
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
                                    if (App.WarViewModel.Field[index + 1] != true &&
                                        App.WarViewModel.Field[index - 1] != true &&
                                        App.WarViewModel.Field[index - 10] != true &&
                                        App.WarViewModel.Field[index - 9] != true &&
                                        App.WarViewModel.Field[index - 11] != true)
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
                                        if (App.WarViewModel.Field[index - 1] != true &&
                                            App.WarViewModel.Field[index - 9] != true &&
                                            App.WarViewModel.Field[index + 9] != true &&
                                            App.WarViewModel.Field[index + 11] != true)
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
                                        if (App.WarViewModel.Field[index + 1] != true &&
                                        App.WarViewModel.Field[index - 1] != true &&
                                        App.WarViewModel.Field[index + 10] != true &&
                                        App.WarViewModel.Field[index + 11] != true &&
                                        App.WarViewModel.Field[index + 9] != true)
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
                                    if (App.WarViewModel.Field[index + 1] != true &&
                                    App.WarViewModel.Field[index + 10] != true
                                   )
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


                                if (App.WarViewModel.Field[index - 1] != true &&
                                App.WarViewModel.Field[index + 9] != true &&
                                App.WarViewModel.Field[index - 11] != true &&
                                App.WarViewModel.Field[index - 10] != true &&
                                App.WarViewModel.Field[index + 10] != true)
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
                                    if (App.WarViewModel.Field[index + 1] != true &&
                                        App.WarViewModel.Field[index - 9] != true &&
                                        App.WarViewModel.Field[index + 9] != true &&
                                        App.WarViewModel.Field[index + 11] != true &&
                                         App.WarViewModel.Field[index - 10] != true)
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
                                    if (App.WarViewModel.Field[index - 1] != true &&
                                    App.WarViewModel.Field[index + 1] != true &&
                                    App.WarViewModel.Field[index - 9] != true &&
                                    App.WarViewModel.Field[index + 9] != true &&
                                    App.WarViewModel.Field[index - 11] != true &&
                                    App.WarViewModel.Field[index + 11] != true)
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

        /// <summary>   Reacts on the user input and calls the methode ResetFIeld. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="sender">   The source of the event; typically <see cref="NavigationHelper"/> </param>
        /// <param name="e">        Routed event information. </param>

        private void Button_Remove(object sender, RoutedEventArgs e)
        {
            ResetField();
        }

        /// <summary>   Reacts on the Userinput and call the async methode Autoplace. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <param name="sender">   The source of the event; typically <see cref="NavigationHelper"/> </param>
        /// <param name="e">        Routed event information. </param>

        private void Button_Auto(object sender, RoutedEventArgs e)
        {
            LoadPopUp();
        }

        /// <summary>   Places all ships on the field automatically. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <returns>   A Task&lt;bool&gt; </returns>

        private async Task<bool> Autoplace()
        {
            ResetField();
            AutoButton.IsEnabled = false;
            int counter = 0;

            while (App.WarViewModel.AllShipsSet == false)
            {
                Random random = new Random();
                var randomNumber = random.Next(0, 100);
                counter++;
                PlaceShip(randomNumber, App.WarViewModel.ShipLength, GetRandomBoolean());
                if (ships.Battleship == 0)
                {
                    App.WarViewModel.WhichShip.SetAllValues(false);
                    App.WarViewModel.WhichShip[1] = true;
                    App.WarViewModel.ShipLength = 4;
                }
                if (ships.Cruiser == 0)
                {
                    App.WarViewModel.WhichShip.SetAllValues(false);
                    App.WarViewModel.WhichShip[2] = true;
                    App.WarViewModel.ShipLength = 3;
                }
                if (ships.Destroyer == 0)
                {
                    App.WarViewModel.WhichShip.SetAllValues(false);
                    App.WarViewModel.WhichShip[3] = true;
                    App.WarViewModel.ShipLength = 2;
                }
                if (counter > 300)
                {
                    await Autoplace();

                }
                await Task.Delay(1);
            }
            AutoButton.IsEnabled = true;
            return true;
        }

        /// <summary>   Resets the whole field and the accompanying properties. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>


        private void ResetField()
        {
            for (int i = 0; i < 100; i++)
            {
                var button =
                    this.ContentRoot.Children.OfType<Button>().Single(x => x.TabIndex == i);
                button.Background = new SolidColorBrush(Colors.LightBlue);
                App.WarViewModel.Field[i] = false;
            }
            App.WarViewModel.WhichShip.SetAllValues(false);
            App.GameViewModel.MyField.SetAllValues(false);
            App.WarViewModel.WhichShip[0] = true;
            App.WarViewModel.AllShipsSet = false;
            App.WarViewModel.ShipLength = 5;
            App.WarViewModel.Battleship = 1;
            App.WarViewModel.Cruiser = 2;
            App.WarViewModel.Destroyer = 3;
            App.WarViewModel.Submarine = 4;
            ships.ResetValues();
        }

        /// <summary>   Returns a random boolean according to who's turn it is. </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>

        public bool GetRandomBoolean()
        {
            if (App.GameViewModel.WhosTurn)
            {
                App.GameViewModel.WhosTurn = false;
                return ships.Battleship != 0 || ships.Cruiser != 0;
            }
            else
            {
                App.GameViewModel.WhosTurn = true;
                return ships.Battleship == 0 && ships.Cruiser == 0;
            }
        }

        /// <summary>   Loads a progressring while autoplacing.   </summary>
        ///
        /// <remarks>   Christopher, 6/19/2015. </remarks>

        private async void LoadPopUp()
        {      
                App.WarViewModel.Oppacity = 0.3;
                App.WarViewModel.Progress = true;
                App.WarViewModel.IsOpen = true;
                await Autoplace();
                App.WarViewModel.Progress = false;
                App.WarViewModel.IsOpen = false;
                App.WarViewModel.Oppacity = 1;   
        }

    }
}
