using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockTrackerCommon.Services;
using StockTrackerCommon.Services.Infrastructure;
using StockTrackerServer;
using System;
using System.Linq;
using System.Threading.Tasks;


using IHost host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
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
            services.AddSingleton<ServerApp>();
        });
}



//class Program
//{ 


//    static async Task Main(string[] args)
//    {
//        var serviceProvider = new ServiceCollection()
//                  .AddSingleton<IDataService, DataService>()
//                  .BuildServiceProvider();

//        var service = serviceProvider.GetService<IDataService>();


//        var users = service.GetAllUsers().Result;

//        foreach ( var user in users )
//        {
//            Console.WriteLine($"user : {user.UserName}");
//        }



//    }


//}