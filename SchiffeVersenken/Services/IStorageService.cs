using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken.Services
{
    /// <summary>
    ///  Interface for persistant storage. 
    /// </summary>
    public interface IStorageService
    {

        Task Write(string fileName, string text);


        Task Write(string fileName, object obj);


        Task<string> Read(string fileName);


        Task<T> Read<T>(string fileName);


        T ReadSetting<T>(T defaultValue = default (T), [CallerMemberName]string key = null);

        void WriteSetting(object obj, [CallerMemberName] string key = null);
    }
}
