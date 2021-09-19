using CommonServiceLocator;
using FormulaRendererApp.Enum;
using FormulaRendererApp.Services;
using FormulaRendererApp.ViewModels;
using FormulaRendererApp.Views;
using GalaSoft.MvvmLight.Ioc;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FormulaRendererApp
{
    public partial class App : Application
    {
        private static Locator locator; 
        public static Locator Locator
        {
            get => locator ?? (locator = new Locator());
        }

        public static IDialogService DialogService { get; set; }

        public static App Instance
        {
            get { return Current as App; }
        }

        public App()
        {
            InitializeComponent();
            DialogService = new DialogService();

            var navigationService = RegisterAndConfigureNavigationService();

            Connectivity.ConnectivityChanged += OnConnectivityChanged;

            var firstPage = new NavigationPage(new FormulaPage());
            navigationService.Initialize(firstPage);
            MainPage = firstPage;
        }
        
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private INavigationService RegisterAndConfigureNavigationService()
        {
            INavigationService navigationService;
            if (!SimpleIoc.Default.IsRegistered<INavigationService>())
            {
                // Setup navigation service:
                navigationService = new NavigationService();

                // Configure pages:
                navigationService.Configure(AppPages.FormulaPage, typeof(FormulaPage));
                navigationService.Configure(AppPages.ImageDisplayPage, typeof(ImageDisplayPage));

                // Register NavigationService in IoC container:
                SimpleIoc.Default.Register(() => navigationService);
            }
            else
            {
                navigationService = SimpleIoc.Default.GetInstance<INavigationService>();
            }
            return navigationService;
        }


        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            switch (e.NetworkAccess)
            {
                case NetworkAccess.None | NetworkAccess.Unknown:
                    DialogService.DisplayAlert(FormulaRendererApp.Resources.ConnectionLostTitle, FormulaRendererApp.Resources.ConnectionLostMessage);
                    break;

                case NetworkAccess.ConstrainedInternet:
                    DialogService.DisplayAlert(FormulaRendererApp.Resources.LimitedConnectionTitle, FormulaRendererApp.Resources.LimitedConnectionMessage);
                    break;

                case NetworkAccess.Internet:
                    DialogService.DisplayToast(FormulaRendererApp.Resources.ConnectionBack, 3);
                    break;
            }
        }
    }
}
