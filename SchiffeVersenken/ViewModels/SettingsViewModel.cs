using GalaSoft.MvvmLight;
using SchiffeVersenken.Services;

namespace SchiffeVersenken.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IStorageService storageService;


        public SettingsViewModel(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        public bool DifficultyEasy
        {
            get { return storageService.ReadSetting(true); }
            set { storageService.WriteSetting(value); }
        }

        public bool DifficultyModerate
        {
            get { return storageService.ReadSetting(true); }
            set { storageService.WriteSetting(value); }
        }

        public bool DifficultyHard
        {
            get { return storageService.ReadSetting(true); }
            set { storageService.WriteSetting(value); }
        }

        public bool Music
        {
            get { return storageService.ReadSetting(true); }
            set { storageService.WriteSetting(value); }
        }

        public bool Soundeffect
        {
            get { return storageService.ReadSetting(true); }
            set { storageService.WriteSetting(value); }
        }
    }
}
