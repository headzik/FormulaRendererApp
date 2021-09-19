using FormulaRendererApp.Exceptions;
using FormulaRendererApp.Services;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FormulaRendererApp.Services
{
    public class FileService : IFileService
    {
        public object Title { get; private set; }

        public async Task<bool> IsStoragePermissionGranted()
        {
            PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            return status == PermissionStatus.Granted;
        }

        public async Task<bool> AskForPermission()
        {
            var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
            return results[Permission.Storage] == PermissionStatus.Granted;
        }

        public Task StoreFile(string fileName, byte[] bytes)
        {
            var directory = DependencyService.Get<IFileSystem>().GetDownloadPath();
            return StoreFile(directory, fileName, bytes);
        }

        public Task StoreFile(string directory, string fileName, byte[] bytes)
        {
            return Task.Run(async () =>
            {
                var permissionGranted = await CheckPermission();
                var path = Path.Combine(directory, fileName);
                using (var file = File.Create(path))
                {
                    await file.WriteAsync(bytes, 0, bytes.Length);
                    file.Flush();
                    file.Close();
                }
            });
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public Task ShareFile(string fileName, byte[] bytes)
        {
            return Task.Run(async () =>
            { 
                var path = Path.Combine(FileSystem.CacheDirectory, fileName);
                File.WriteAllBytes(path, bytes);
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = Resources.SharingTitle,
                    File = new ShareFile(path)
                });
            });
        }

        private async Task<bool> CheckPermission()
        {
            var permissionGranted = await IsStoragePermissionGranted();
            if (!permissionGranted)
            {
                permissionGranted = await AskForPermission();
            }

            return permissionGranted;
        }
    }
}

