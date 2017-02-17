using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken.ExtensionMethods
{
    /// <summary>
    ///  ExtensionMethodes
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        ///  Sets all values to a specific value.
        /// </summary>
        public static T[] SetAllValues<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
            return array;
        }

        /// <summary>
        ///  CHecks if all values in an array are the same.
        /// </summary>
        public static bool CheckAllValues<T>(this T[] array, T value)
        {
            return array.All(t => Equals(t, value));
        }
    }
}
