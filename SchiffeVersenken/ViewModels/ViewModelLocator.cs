using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace SchiffeVersenken.ViewModels
{
    internal class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<INavigationService, NavigationService>();
            }
            SimpleIoc.Default.Register<SplashScreenViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<WarViewModel>();
            SimpleIoc.Default.Register<GameViewModel>();
            SimpleIoc.Default.Register<HighscoreViewModel>();
        }

        public SplashScreenViewModel SplashScreenViewModel
        {
            get { return ServiceLocator.Current.GetInstance<SplashScreenViewModel>(); }
        }

        public HighscoreViewModel HighscoreViewModel
        {
            get { return ServiceLocator.Current.GetInstance<HighscoreViewModel>(); }
        }

        public MainViewModel MainViewModel
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }
        public SettingsViewModel SettingsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<SettingsViewModel>(); }
        }
        public WarViewModel WarViewModel
        {
            get { return ServiceLocator.Current.GetInstance<WarViewModel>(); }
        }
        public GameViewModel GameViewModel
        {
            get { return ServiceLocator.Current.GetInstance<GameViewModel>(); }
        }
    }
}
