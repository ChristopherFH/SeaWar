using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SchiffeVersenken.Services
{
    class WinRtStorageService : PortableStorageService
    {
        /// <summary>
        ///  Task to write a string to a file.
        /// </summary>
        public override async Task Write(string fileName, string text)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, text);
        }

        /// <summary>
        ///  Task to read a string from a file. 
        /// </summary>
        public override async Task<string> Read(string fileName)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.GetFileAsync(fileName);
            return await FileIO.ReadTextAsync(file);
        }

    }
}
