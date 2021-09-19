using FormulaRendererApp.iOS.Services;
using FormulaRendererApp.Services;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileSystem))]

namespace FormulaRendererApp.iOS.Services
{
    class FileSystem : IFileSystem
    {
        public string GetDownloadPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public string GetDownloadName()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString();
        }
    }
}