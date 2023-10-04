using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace StockTrackerCommon.Helpers
{
    public static class EncryptionHelper
    {

        //Set Static variables of the Key and the Salt as these will always be the same - Key is a Randomly generated GUID
        private static string encryptionKey = "df1c39f3-8a3a-4ac7-9a5c-5b86b7d54f6d";
        private static byte[] encryptionSalt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

        /// <summary>
        /// Method which takes in an string and then encrypt it using the encryption key and salt
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns></returns>
        public static string Encrypt(string unencryptedText)
        {
            string encryptedText = String.Empty;
            byte[] unencryptedBytes = Encoding.Unicode.GetBytes(unencryptedText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, encryptionSalt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(unencryptedBytes, 0, unencryptedBytes.Length);
                        cs.Close();
                    }
                    encryptedText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptedText;
        }


        /// <summary>
        /// Method which takes in an encrypted string and then decrypts it using the encryption key and salt
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string Decrypt(string encryptedText)
        {
            string unencryptedText = String.Empty;
            encryptedText = encryptedText.Replace(" ", "+");
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, encryptionSalt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                        cs.Close();
                    }
                    unencryptedText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return unencryptedText;
        }
    }
}
