using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerCommonTests.Helpers
{
    public class HashingHelperTests
    {
        [Fact]
        public void testHashing()
        {
            //Arrange
            string stringToHash = "test";
            string hashSalt = HashingHelper.GenerateHashSalt();

            //Act
            string hashedString1 = HashingHelper.Hasher(stringToHash, hashSalt);
            string hashedString2 = HashingHelper.Hasher(stringToHash, hashSalt);

            //Assert
            //check the hashed strings are not null
            Assert.NotNull(hashedString1);
            Assert.NotNull(hashedString2);
            //check the hashed strings are not the same as the unhashed string
            Assert.NotEqual(stringToHash, hashedString1);
            Assert.NotEqual(stringToHash, hashedString2);
            //check the hashed strings are the same
            Assert.Equal(hashedString1, hashedString2);
        }
    }
}
