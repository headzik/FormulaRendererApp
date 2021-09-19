using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormulaRendererApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageDisplayPage : ContentPage
    {
        public ImageDisplayPage(MemoryStream imageStream)
        {
            InitializeComponent();
            BindingContext = App.Locator.ImageDisplayViewModel;
            App.Locator.ImageDisplayViewModel.ImageStream = imageStream;
        }
    }
}