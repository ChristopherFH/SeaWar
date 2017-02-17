using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using SchiffeVersenken.Models;

namespace SchiffeVersenken.ViewModels
{
    public class HighscoreViewModel : ViewModelBase
    {
       
        public HighscoreViewModel()
        {      
            Highscore = new ObservableCollection<Highscores>();
           // Highscore.Add(new Highscores("Fürst Finster", "Easy", 71, "10"));
            //Highscore.Add(new Highscores("Tom Turbo", "Easy", 89));
        }

        public ObservableCollection<Highscores> Highscore { get; set; }
       
    }
}
