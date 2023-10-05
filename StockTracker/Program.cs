using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StockTracker.UserControls;
using StockTracker.Services.Infrastructure;
using StockTracker.Services;

namespace StockTracker
{
    internal static class Program
    {

        public static IServiceProvider ServiceProvider { get; private set; }


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            ApplicationConfiguration.Initialize();
            Application.Run(ServiceProvider.GetRequiredService<frmStockTracker>());
            //Application.Run(new frmStockTracker());
        }

        /// <summary>
        /// I used this https://stackoverflow.com/questions/70475830/how-to-use-dependency-injection-in-winforms to help with the implementation of dependency injection in Winforms
        /// </summary>
        /// <returns></returns>
        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services.AddTransient<frmStockTracker>();
                    services.AddTransient<ctrLogin>();
                    services.AddSingleton<IClientTransportService, NetmqClientTransportService>();
                    //services.AddSingleton<IClientTransportService, ClientTransportService>();
                    services.AddSingleton<IUserService, UserService>();
                });
        }

    }
}