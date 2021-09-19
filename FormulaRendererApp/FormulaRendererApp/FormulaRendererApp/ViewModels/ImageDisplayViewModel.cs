using System;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
using FormulaRendererApp.Services;
using System.Threading.Tasks;
using Xamarin.Essentials;
using GalaSoft.MvvmLight;

namespace FormulaRendererApp.ViewModels
{
    public class ImageDisplayViewModel : ViewModelBase
    {

        private MemoryStream imageStream;
        public MemoryStream ImageStream
        {
            get => imageStream;
            set
            {
                if(value != null)
                {
                    imageStream = value;
                    ImageSource = ImageSource.FromStream(() => imageStream);
                }
            }
        }

        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get => imageSource;
            set
            {
                imageSource = value;
                RaisePropertyChanged();
            }
        }

        public IFileService FileService { get; }
        public ICommand ShareCommand { protected set; get; }
        public ICommand StoreCommand { protected set; get; }
        
        public ImageDisplayViewModel(IFileService fileService)
        {
            FileService = fileService;

            ShareCommand = new Command(() => ShareImage());
            StoreCommand = new Command(() => StoreImage());
        }

        private async void ShareImage()
        {
            try
            {
                using (var task = await App.DialogService.DisplayProgress(() =>
                    FileService.ShareFile(DateTime.Now.Ticks + "formula.png", ImageStream.ToArray())))
                {
                    switch (task.Status)
                    {
                        case TaskStatus.RanToCompletion:
                            //DialogService.DisplayToast(Resources.SuccessfulShare, 3);
                            break;
                        case TaskStatus.Faulted:
                            App.DialogService.DisplayGeneralError();
                            Debug.Write(task.Exception.ToString());
                            break;
                    }
                }
            }
            catch (TaskCanceledException) { }
            catch (Exception e)
            {
                //not a very clean approach but allows me to handle native exceptions exceptions
                if (e.InnerException.GetType().ToString().Equals("Java.Net.UnknownHostException"))
                {
                    switch (Connectivity.NetworkAccess)
                    {
                        case NetworkAccess.None | NetworkAccess.Unknown:
                            App.DialogService.DisplayAlert(Resources.ConnectionLostTitle, Resources.ConnectionLostMessage);
                            break;

                        case NetworkAccess.ConstrainedInternet:
                            App.DialogService.DisplayAlert(Resources.LimitedConnectionTitle, Resources.LimitedConnectionMessage);
                            break;
                    }
                }
                else
                {
                    App.DialogService.DisplayGeneralError();
                }
            }
        }

        private async void StoreImage()
        {
            try
            {
                using (var task = await App.DialogService.DisplayProgress(() => 
                    FileService.StoreFile(DateTime.Now.Ticks + "formula.png", ImageStream.ToArray())))
                {
                    switch (task.Status)
                    {
                        case TaskStatus.RanToCompletion:
                            App.DialogService.DisplayToast(Resources.SuccessfulFileStorage + DependencyService.Get<IFileSystem>().GetDownloadName(), 3);
                            break;
                        case TaskStatus.Faulted:
                            throw task.Exception.InnerException;
                    }
                }
            }
            catch (TaskCanceledException) { }
            catch (UnauthorizedAccessException)
            {
                App.DialogService.DisplayAlert(Resources.PermissionNotGrantedTitle, Resources.PermissionNotGrantedMessage);
            }
            catch (Exception)
            {
                App.DialogService.DisplayGeneralError();
            }
        }
    }
}
