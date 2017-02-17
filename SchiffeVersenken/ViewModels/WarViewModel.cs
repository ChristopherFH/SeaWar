using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using SchiffeVersenken.ExtensionMethods;
using SchiffeVersenken.Models;
using SchiffeVersenken.Services;

namespace SchiffeVersenken.ViewModels
{
    public class WarViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IDialogService dialogService;

        public WarViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            InitCommands();
            InitVariables();            
        }

        private void InitVariables()
        {
            ShipLength = 5;
            Direction = "←";
            Field = new bool[100].SetAllValues(false);
            WhichShip = new bool[4].SetAllValues(false);
            WhichShip[0] = true;
            AllShipsSet = false;
            Progress = false;
            IsOpen = false;
            Oppacity = 1;
            Battleship = 1;
            Cruiser = 2;
            Destroyer = 3;
            Submarine = 4; 
        }

        public void InitCommands()
        {
            SelectBattleshipCommand = new RelayCommand(SelectBattleship);
            SelectCruiserCommand = new RelayCommand(SelectCruiser);
            SelectSubmarineCommand = new RelayCommand(SelectSubmarine);
            SelectDestroyerCommand = new RelayCommand(SelectDestroyer);
            RemoveLastCommand = new RelayCommand(RemoveLast);
            ChangeDirectionCoommand = new RelayCommand(ChangeDirection);
            StartGameCommand = new RelayCommand(StartWar);      
        }

        public ICommand SelectBattleshipCommand { get; set; }
        public ICommand StartGameCommand { get; set; }
        public ICommand ChangeDirectionCoommand { get; set; }
        public ICommand RemoveLastCommand { get; set; }
        public ICommand SelectCruiserCommand { get; set; }
        public ICommand SelectSubmarineCommand { get; set; }
        public ICommand SelectDestroyerCommand { get; set; }

        public bool[] WhichShip { get; set; }
        public int ShipLength { get; set; }       
        public int PreviousShip { get; set; }
        public bool[] Field { get; set; }
        public int Index { get; set; }
        public string Direction { get; set; }
        public bool AllShipsSet { get; set; }
        public bool Progress { get; set; }
        public object CommandParameter { get; set; }
        public bool IsOpen { get; set; }
        public double Oppacity { get; set; }
        public int Battleship { get; set; }
        public int Cruiser { get; set; }
        public int Destroyer { get; set; }
        public int Submarine { get; set; }

        private void SelectDestroyer()
        {
            ShipLength = 3;
            WhichShip.SetAllValues(false);
            WhichShip[2] = true;
        }
        private void SelectSubmarine()
        {
            ShipLength = 2;
            WhichShip.SetAllValues(false);
            WhichShip[3] = true;
        }
        private void SelectCruiser()
        {
            ShipLength = 4;
            WhichShip.SetAllValues(false);
            WhichShip[1] = true;
        }
        private void SelectBattleship()
        {
            ShipLength = 5;
            WhichShip.SetAllValues(false);
            WhichShip[0] = true;
        }

        private void RemoveLast()
        {
            for (int i = 0; i < 5; i++)
            {
                Field[Index] = false;
                Index--;
            }
        }

        private void ChangeDirection()
        {
            if (Direction.Equals("←"))
            {
                Direction = "↑";
            }
            else if (Direction.Equals("↑"))
            {
                Direction = "←";
            }
        }

        private void StartWar()
        {
            if (AllShipsSet)
            {
                navigationService.NavigateTo(Constants.Constants.GamePage);
            }
            else
            {
                dialogService.ShowMessage("Place all ships!", "");
            }
        }    
    }
}
