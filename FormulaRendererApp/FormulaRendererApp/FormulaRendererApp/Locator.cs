using CommonServiceLocator;
using FormulaRendererApp.Services;
using FormulaRendererApp.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaRendererApp
{
    public class Locator
    {
      
        public Locator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<FormulaViewModel>();
            SimpleIoc.Default.Register<ImageDisplayViewModel>();
            SimpleIoc.Default.Register<IRestService>(() => new RestService());
            SimpleIoc.Default.Register<IDialogService>(() => new DialogService());
            SimpleIoc.Default.Register<IFileService>(() => new FileService());
            SimpleIoc.Default.Register<FormulaManager>();
        }

        public FormulaViewModel FormulaViewModel => ServiceLocator.Current.GetInstance<FormulaViewModel>();        
        public ImageDisplayViewModel ImageDisplayViewModel => ServiceLocator.Current.GetInstance<ImageDisplayViewModel>();

        //might be done automatically
        public static void Cleanup()
        {
            SimpleIoc.Default.Unregister<FormulaViewModel>();
            SimpleIoc.Default.Unregister<ImageDisplayViewModel>();
            SimpleIoc.Default.Unregister<IRestService>();
            SimpleIoc.Default.Unregister<IDialogService>();
            SimpleIoc.Default.Unregister<IFileService>();
            SimpleIoc.Default.Unregister<FormulaManager>();
        }
    }
}
