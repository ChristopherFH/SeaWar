using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using SchiffeVersenken.Models;

namespace SchiffeVersenken.Converters
{ 
    /// <summary>
    ///  Click on the username changes it to a string and transfers it to the viemodel.
    /// </summary>
    class ClickToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var args = value as ItemClickEventArgs;
            if (args == null) return null;
            return args.ClickedItem as UserList;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
