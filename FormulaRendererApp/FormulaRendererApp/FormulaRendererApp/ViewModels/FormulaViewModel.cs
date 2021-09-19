using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using FormulaRendererApp.Models;
using System.Threading.Tasks;
using FormulaRendererApp.Enum;
using GalaSoft.MvvmLight;
using FormulaRendererApp.Services;
using Xamarin.Essentials;
using FormulaRendererApp.Exceptions;

namespace FormulaRendererApp.ViewModels
{
    public class FormulaViewModel : ViewModelBase
    {
        private string formula;
        public string Formula
        {
            set
            {
                if (formula != value)
                {
                    formula = value;
                    RaisePropertyChanged();
                }
            }

            get => formula;
        }

        private readonly INavigationService NavigationService;
        private readonly FormulaManager FormulaManager;
        public ICommand RenderCommand { protected set; get; }

        public FormulaViewModel(INavigationService navigationService, FormulaManager formulaManager) 
        {
            NavigationService = navigationService;
            FormulaManager = formulaManager;
            RenderCommand = new Command(() => RenderImage());
        }

        private async void RenderImage()
        {
            if (string.IsNullOrWhiteSpace(Formula))
            {
                App.DialogService.DisplayAlert(Resources.EmptyFormulaTitle, Resources.EmptyFormulaMessage);
            }
            else
            {
                try
                {
                    using (var task = await App.DialogService.DisplayProgress(() => 
                               FormulaManager.RenderImageFromFormula(formula)))
                    {
                        switch (task.Status)
                        {
                            case TaskStatus.RanToCompletion:
                                var imageStream = task.Result;
                                NavigationService.NavigateTo(AppPages.ImageDisplayPage, imageStream);
                                break;
                            case TaskStatus.Faulted:
                                throw task.Exception.InnerException;
                        }
                    }
                }
                catch (TaskCanceledException) { }
                catch (BadRequestException)
                {
                    App.DialogService.DisplayAlert(Resources.WrongFormulaTitle, Resources.WrongFormulaMessage);
                }
                catch (InternalServerErrorException)
                {
                    App.DialogService.DisplayAlert(Resources.InternalServerErrorTitle, Resources.InternalServerErrorMessage);
                }
                catch (Exception e)
                {
                    //not a very clean approach but allows me to handle native exceptions
                    if(e.InnerException.GetType().ToString().Equals("Java.Net.UnknownHostException"))
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
        }

    }
}
