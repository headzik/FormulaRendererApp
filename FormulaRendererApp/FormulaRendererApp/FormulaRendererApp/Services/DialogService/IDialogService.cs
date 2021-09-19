using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FormulaRendererApp.Services
{
    public interface IDialogService
    {
       void DisplayAlert(string title, string message);
       void DisplayToast(string message, int durationInSeconds);
       void DisplayGeneralError();

        //perhaps there is a way to have one function for dealing with generic type and no type, for now two separate functions were necessary
        //for function returning a type
        Task<Task<T>> DisplayProgress<T>(Func<Task<T>> function);

        //task does not return anything
        Task<Task> DisplayProgress(Func<Task> function);


    }
}
