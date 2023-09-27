using StockTrackerCommon.Models;
using StockTrackerCommon.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerServer
{
    public class ServerApp
    {
        private readonly IDataService _dataService;
        public ServerApp(IDataService dataService) 
        {
            _dataService = dataService;
        }

        public void Run(string[] args)
        {
            User[] users = _dataService.GetAllUsers().Result.ToArray();
            foreach(User user in users)
            {
                Console.WriteLine($"User : {user.UserName}");
            }
        }

    }
}
