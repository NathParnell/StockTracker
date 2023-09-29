using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerCommon.Database
{
    public static class StockTrackerDbContextSeedData
    {
        static object synchlock = new object();
        static volatile bool seeded = false;

        /// <summary>
        /// We need to prefill all of the data within the in memory database
        /// This method takes generated data and adds it to the in memory database
        /// </summary>
        public static void EnsureSeedData(this StockTrackerDbContext context)
        {
            if (!seeded && context.Users.Count() == 0)
            {
                lock (synchlock)
                {
                    if (!seeded)
                    {
                        var users = GenerateUsers();

                        context.Users.AddRange(users);

                        context.SaveChanges();
                        seeded = true;
                    }
                }
            }
        }

        #region "Generate Data Methods"
        /// <summary>
        /// This method generates Users for the mock database
        /// </summary>
        /// <returns></returns>
        private static User[] GenerateUsers()
        {
            return new User[]
            {
                new User
                {
                    UserId = Taikandi.SequentialGuid.NewGuid().ToString(),
                    UserName = "Salon1",
                    Password = "1234"
                },
                new User
                {
                    UserId = Taikandi.SequentialGuid.NewGuid().ToString(),
                    UserName = "Salon2",
                    Password = "1234"
                },
                new User
                {
                    UserId = Taikandi.SequentialGuid.NewGuid().ToString(),
                    UserName = "Salon3",
                    Password = "1234"
                },
                new User
                {
                    UserId = Taikandi.SequentialGuid.NewGuid().ToString(),
                    UserName = "SalonSupplier",
                    Password = "1234"
                }
            };
        }
        #endregion
    }
}
