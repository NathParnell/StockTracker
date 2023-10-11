using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerCommon.Helpers
{
    public static class HashingHelper
    {
        /// <summary>
        /// Method Generates a hash salt, returning it as a string
        /// </summary>
        /// <returns></returns>
        public static string GenerateHashSalt()
        {
            const int saltSize = 128;
            RNGCryptoServiceProvider hashProvider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[saltSize];
            hashProvider.GetBytes(salt);
            return Encoding.Default.GetString(salt);
        }

        /// <summary>
        /// Method takes in a hash salt and a string. We then hash the password using the salt
        /// And we return the hashed string
        /// </summary>
        /// <param name="stringToHash"></param>
        /// <param name="saltString"></param>
        /// <returns></returns>
        public static string Hasher(string stringToHash, string saltString)
        {
            const int HashSize = 128;
            const int Iterations = 100000;

            byte[] salt = Encoding.Default.GetBytes(saltString);

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(stringToHash, salt, Iterations);
            string hashedString = Encoding.Default.GetString(pbkdf2.GetBytes(HashSize));

            return hashedString;
        }
    }
}
