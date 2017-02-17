using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using SchiffeVersenken.ExtensionMethods;

namespace SchiffeVersenken.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
     
        public GameViewModel()
        {    
            ShipLength = 5;
            EnemyField = new bool[100].SetAllValues(false);
            MyField = new bool[100].SetAllValues(false);
            WhichShip = new bool[4].SetAllValues(false);
            WhichShip[0] = true;
            AllShipsSet = false;     
            Moves = 0;
            Index = 0;
            GameOver = false;
            WhosTurn = GetRandomBoolean();
            Arrow = WhosTurn ? 180 : 0;   
           DisplyTime = new Stopwatch();
        }
          

        public Stopwatch DisplyTime { get; set; }
        public string Time { get; set; }
        public bool GameOver { get; set; }      
        public int Arrow { get; set; }
        public int Index { get; set; }
        public int ShipLength { get; set; }
        public bool WhosTurn { get; set; }
        public int Moves { get; set; } 
        public bool AllShipsSet { get; set; }
        public bool[] WhichShip { get; set; }
        public bool[] EnemyField { get; set; }
        public bool[] MyField { get; set; }

        private bool GetRandomBoolean()
        {
            Random rnd = new Random();
            return rnd.Next(0, 2) == 0;
        }

    }
}
