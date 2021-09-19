using Acr.UserDialogs;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FormulaRendererApp.Services
{
    public class DialogService : IDialogService
    {

        private const int TIMEOUT_SECONDS = 10;

        public void DisplayAlert(string title, string message)
        {
            UserDialogs.Instance.Alert(new AlertConfig()
            {
                Message = message,
                Title = title
            });
        }

        public void DisplayGeneralError()
        {
            DisplayAlert(Resources.Error, Resources.SomethingWentWrongMessage);
        }
        
        public async Task<Task<T>> DisplayProgress<T>(Func<Task<T>> function)
        {
            var cancelSource = new CancellationTokenSource(TimeSpan.FromSeconds(TIMEOUT_SECONDS));
            using (UserDialogs.Instance.Progress(GetProgressConfig(cancelSource.Cancel)))
            {
                var task = function.Invoke();

                while (!(task.IsCompleted))
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), cancelSource.Token);
                }

                return task;
            }
        }

        //task does not return anything
        public async Task<Task> DisplayProgress(Func<Task> function)
        {
            var cancelSource = new CancellationTokenSource(TimeSpan.FromSeconds(TIMEOUT_SECONDS));
            using (UserDialogs.Instance.Progress(GetProgressConfig(cancelSource.Cancel)))
            {
                var task = function.Invoke();

                while (!(task.IsCompleted))
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), cancelSource.Token);
                }

                return task;
            }
        }

        public void DisplayToast(string message, int durationInSeconds)
        {
            UserDialogs.Instance.Toast(new ToastConfig(message)
            {
                BackgroundColor = Color.Black.MultiplyAlpha(0.5),
                MessageTextColor = Color.White,
                Duration = TimeSpan.FromSeconds(durationInSeconds)                
            });
        }

        private ProgressDialogConfig GetProgressConfig(Action cancel)
        {
            return new ProgressDialogConfig()
            {
                Title = Resources.WaitMessage,
                CancelText = "\n" + Resources.CancelTitle,
                MaskType = MaskType.Black,
                OnCancel = cancel,
                IsDeterministic = false
            };
        }
    }
}
