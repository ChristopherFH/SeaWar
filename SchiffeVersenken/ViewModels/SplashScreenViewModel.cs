using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace SchiffeVersenken.ViewModels
{
    class SplashScreenViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;

        public SplashScreenViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }    
    }
}
