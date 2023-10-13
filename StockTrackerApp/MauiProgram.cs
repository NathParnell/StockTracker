using Microsoft.Extensions.Logging;
using StockTrackerApp.Data;
using StockTrackerApp.Services;
using StockTrackerApp.Services.Infrastructure;

namespace StockTrackerApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<ISupplierService, SupplierService>();
            builder.Services.AddSingleton<IClientTransportService, NetmqClientTransportService>();

            return builder.Build();
        }
    }
}
