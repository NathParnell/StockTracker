using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerServer.Services.Infrastructure
{
    public interface IDataService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserByUsername(string username);
    }
}
