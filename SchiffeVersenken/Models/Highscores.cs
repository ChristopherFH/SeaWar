using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace SchiffeVersenken.Models
{

    /// <summary>
    ///  This class contains all neccessary highscore properties. 
    /// </summary>
    public class Highscores : ObservableObject
    {
        public Highscores()
        {
            
        }

        /// <summary>
        ///  Initializes all properties. 
        /// </summary>
        public Highscores(string name, string difficulty, int score, string elapsedTime)
        {
            Score = score;
            Name = name;
            Difficulty = difficulty;
            Time = elapsedTime;
        }

        public string Name { get; set; }

        public int Score { get; set; }

        public string Difficulty { get; set; }

        public string Time { get; set; }

    }
}
