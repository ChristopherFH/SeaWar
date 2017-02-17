using SchiffeVersenken.ExtensionMethods;

namespace SchiffeVersenken.Models
{
    /// <summary>
    ///  This class contains all neccessary properties for the game. 
    /// </summary>
    public class Ships
    {
        /// <summary>
        ///  Initializes the ships with the given quantitiy. 
        /// </summary>
        public Ships()
        {
            Destroyer = 3;
            Cruiser = 2;
            Battleship = 1;
            Submarine = 4;
            AllShipsPlaced = false;
        }

        public int Battleship { get; set; }
        public int Cruiser { get; set; }
        public int Destroyer { get; set; }
        public int Submarine { get; set; }
        public bool AllShipsPlaced { get; set; }


        /// <summary>
        ///  Checks if a ship is set. 
        /// </summary>
        public bool IsSet(bool[] ship)
        {
            if (ship[0] == true && Battleship == 0)
            {
                return true;
            }
            else if (ship[1] == true && Cruiser == 0)
            {
                return true;
            }
            else if (ship[2] == true && Destroyer == 0)
            {
                return true;
            }
            else if( ship[3] == true && Submarine == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///  Sets a ship and reduces the quantity of it. 
        /// </summary>
        public void ShipSet(bool[] ship)
        {
            if (ship[0] == true)
            {
                this.Battleship--;
            }else if (ship[1] == true)
            {
                 this.Cruiser--;
            }else if (ship[2] == true)
            {
                this.Destroyer--;
            }
            else
            {
                this.Submarine--;
            }
            if (Battleship == 0 && Cruiser == 0 && Submarine == 0 && Destroyer == 0)
            {
                AllShipsPlaced = true;
            }                      
        }

        /// <summary>
        ///  Unsets a ship and increments the ship quantity. 
        /// </summary>
        public void ShipUnset(bool[] ship)
        {
            if (ship[0] == true)
            {
                this.Battleship++;
            }
            else if (ship[1] == true)
            {
                this.Cruiser++;
            }
            else if (ship[2] == true)
            {
                this.Destroyer++;
            }
            else
            {
                this.Submarine++;
            }
        }

        /// <summary>
        ///  Resets all Values. 
        /// </summary>
        public void ResetValues()
        {
            Destroyer = 3;
            Cruiser = 2;
            Battleship = 1;
            Submarine = 4;
            AllShipsPlaced = false;
        }
    }   
}
