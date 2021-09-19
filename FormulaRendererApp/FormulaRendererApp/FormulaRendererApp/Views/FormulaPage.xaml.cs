using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormulaRendererApp.Views
{    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormulaPage : ContentPage
    {
        public FormulaPage()
        {
            InitializeComponent();
            BindingContext = App.Locator.FormulaViewModel;
        }
    }
}
