using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace SchiffeVersenken.Models
{

    /// <summary>
    ///  This class contains the neccessary user properties. 
    /// </summary>
    public class UserList : ObservableObject
    {
        public UserList()
        {
            
        }

        /// <summary>
        ///  Initialize the username with the given string. 
        /// </summary>
        public UserList(string user)
        {
            Name = user;
        }
    
        public string Name { get; set; }     
    }
}
