using FormulaRendererApp.Droid;
using FormulaRendererApp.Services;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileSystem))]
namespace FormulaRendererApp.Droid
{
    class FileSystem : IFileSystem
    {
        public string GetDownloadPath()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
        }

        public string GetDownloadName()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString();
        }

    }
}