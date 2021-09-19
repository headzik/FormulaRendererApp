using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FormulaRendererApp.Services
{
    public interface IFileService
    {
        Task<bool> IsStoragePermissionGranted();
        Task<bool> AskForPermission();
        Task StoreFile(string fileName, byte[] bytes);
        Task StoreFile(string directory, string fileName, byte[] bytes);
        Task ShareFile(string fileName, byte[] bytes);
        void DeleteFile(string path);
        
    }
}

