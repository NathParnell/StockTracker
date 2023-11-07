using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;

namespace StockTrackerApp
{
    public partial class App : Application
    {
        private IAuthorizationService _authorizationService;
        private ICustomerService _customerService;
        private ISupplierService _supplierService;
        public App(IAuthorizationService authorizationService, ICustomerService customerService, ISupplierService supplierService)
        {
            InitializeComponent();

            MainPage = new MainPage();

            _authorizationService = authorizationService;
            _customerService = customerService;
            _supplierService = supplierService;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            Window window = base.CreateWindow(activationState);

#if WINDOWS
        window.Created += (s, e) =>
        {

            //we need this to use Microsoft.UI.Windowing functions for our window
            var handle = WinRT.Interop.WindowNative.GetWindowHandle(window.Handler.PlatformView);
            var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);

            //and here it is
            appWindow.Closing += async (s, e) =>
            {
                e.Cancel = true;
                bool result = await App.Current.MainPage.DisplayAlert(
                    "Alert title",
                    "You sure want to close app?",
                    "Yes",
                    "Cancel");

                if (result)
                {
                    if (_authorizationService.IsLoggedIn)
                    {
                        if (_authorizationService.UserType == UserType.Supplier){
                            _supplierService.Logout();
                        }
                        else if (_authorizationService.UserType == UserType.Customer){
                            _customerService.Logout();
                        }
                    }
                    App.Current.Quit();
                }
            };
        };
#endif
            return window;
        }

    }
}
