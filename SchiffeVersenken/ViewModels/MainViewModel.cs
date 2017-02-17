using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.Media.Editing;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using SchiffeVersenken.Models;

namespace SchiffeVersenken.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IDialogService dialogService;


        public MainViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            InitCommands();
            Users = new ObservableCollection<UserList>();
        }

        private void InitCommands()
        {
            NavigateToWarCommand = new RelayCommand(NavigateToWarPage);
            SettingsCommand = new RelayCommand(NavigateToSettingsPage);
            NewUserCommand = new RelayCommand(AddNewUserFlyOut);
            ChangeUserCommand = new RelayCommand<UserList>(SelectUserFlyOut);
            NavigateToHighscoreCommand = new RelayCommand(NavigateToHighscore);
        }

      
        public ObservableCollection<UserList> Users { get; set; }

        public string Username { get; set; }

        public ICommand NavigateToHighscoreCommand { get; set; }
        public ICommand NavigateToWarCommand { get; set; }
        public ICommand ChangeUserCommand { get; set; }
        public ICommand NewUserCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

        private void NavigateToSettingsPage()
        {
            navigationService.NavigateTo(Constants.Constants.SettingsPage);      
        }

        private void NavigateToHighscore()
        {
            navigationService.NavigateTo(Constants.Constants.HighscorePage); 
        }

        private void NavigateToWarPage()
        {
            if (!string.IsNullOrEmpty(Username) && (Users.ToList().Exists(x => x.Name == Username)))
            {
                navigationService.NavigateTo(Constants.Constants.WarPage);
            }
            else
            {
                dialogService.ShowMessage("Please select a username!", "");
            }
        }

        private void AddNewUserFlyOut()
        {
            if (!string.IsNullOrWhiteSpace(Username))
            {
                if (!Users.ToList().Exists(x => x.Name == Username))
                {
                    Users.Add(new UserList(Username));
                }
                else
                {
                    dialogService.ShowMessage("Username already exists!", "");
                }
            }
            else
            {
                dialogService.ShowMessage("No User created!", "");
            }
        }

        private void SelectUserFlyOut(UserList name)
        {
            Username = name.Name;
        }
    }
}
