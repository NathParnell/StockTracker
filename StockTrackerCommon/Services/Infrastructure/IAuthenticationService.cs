﻿using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerCommon.Services.Infrastructure
{
    public interface IAuthenticationService
    {
        User? AuthenticateUser(string username, string password);
    }
}
