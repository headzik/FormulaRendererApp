using FormulaRendererApp.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FormulaRendererApp.Services
{
    public interface INavigationService
    {
        string CurrentPageKey { get; }
        void GoBack();
        void NavigateTo(AppPages pageKey);
        void NavigateTo(AppPages pageKey, object parameter);
        void Configure(AppPages pageKey, Type pageType);
        void Initialize(NavigationPage navigation);
    }
}
