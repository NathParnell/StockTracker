using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerCommonTests.Helpers
{
    public class EncryptionHelperTests
    {
        [Fact]
        public void testEncryptingString()
        {
            //Arrange
            string unencryptedText = "textToEncrypt";

            //Act
            string encryptedText = EncryptionHelper.Encrypt(unencryptedText);

            //Assert
            //check the encrypted text is not null
            Assert.NotNull(encryptedText);
            //check the encrypted text is not the same as the unencrypted text
            Assert.NotEqual(unencryptedText, encryptedText);
        }

        [Fact]
        public void testDecryptingString()
        {
            //Arrange
            string unencryptedText = "textToDecrypt";

            //Act
            string encryptedText = EncryptionHelper.Encrypt(unencryptedText);
            string decryptedText = EncryptionHelper.Decrypt(encryptedText);

            //Assert
            //check the decrypted text is not null
            Assert.NotNull(decryptedText);
            //check the decrypted text is the same as the unencrypted text
            Assert.Equal(unencryptedText, decryptedText);
            //check the decrypted text is not the same as the encrypted text
            Assert.NotEqual(encryptedText, decryptedText);
        }
    }
}
