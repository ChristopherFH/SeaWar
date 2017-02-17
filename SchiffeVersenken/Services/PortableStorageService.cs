using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SchiffeVersenken.Services
{
    public abstract class PortableStorageService : IStorageService
    {
        public abstract Task Write(string fileName, string text);

        /// <summary>
        ///  Task to write an object to a file after serialization.
        /// </summary>
        public async Task Write(string fileName, object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            await Write(fileName, json);
        }

        public abstract Task<string> Read(string fileName);

        /// <summary>
        ///  Task to read an object from a file with deserialization afterwards.
        /// </summary>
        public async Task<T> Read<T>(string fileName)
        {
            string json = await Read(fileName);
            return JsonConvert.DeserializeObject<T>(json);
        }


        /// <summary>
        ///  Task to store the settings. 
        /// </summary>
        public void WriteSetting(object obj, [CallerMemberName] string key = null)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values[key] = obj;
        }

        /// <summary>
        ///  Task to read the settings.
        /// </summary>
        public T ReadSetting<T>(T defaultValue = default (T), [CallerMemberName]string key = null)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.Keys.Contains(key))
            {
                return (T)localSettings.Values[key];
            }
            return defaultValue;
        }

    }
}
