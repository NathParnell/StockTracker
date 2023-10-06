using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockTrackerServer;
using StockTrackerServer.Services;
using StockTrackerServer.Services.Infrastructure;


using IHost host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();

//setup log4net based on the log4net config file
log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

var services = scope.ServiceProvider;

try
{
    //goto the run method in the ServerApp class
    services.GetRequiredService<ServerApp>().Run(args);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

IHostBuilder CreateHostBuilder(string[] strings)
{
    return Host.CreateDefaultBuilder()
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<IServerTransportService, NetmqServerTransportService>();
            //services.AddSingleton<IServerTransportService, ServerTransportService>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IRequestService, RequestService>();
            services.AddSingleton<ServerApp>();
        });
}