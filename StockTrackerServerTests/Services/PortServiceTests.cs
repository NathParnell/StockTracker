using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerServerTests.Services
{
    public class PortServiceTests
    {

        [Fact]
        public void testGeneratingValidClientRequestPort()
        {
            //Arrange
            PortService portService = new PortService();
            string clientId = "test";
            string ipAddress = "127.0.0.1";

            //Act
            string port = portService.GenerateClientRequestPort(clientId, ipAddress);

            //Assert
            Assert.NotNull(port);
            Assert.InRange(Int32.Parse(port), 49152, 57343);
        }

        [Fact]
        public void testGeneratingValidClientMessagingPort()
        {
            //Arrange
            PortService portService = new PortService();
            string clientId = "test";
            string ipAddress = "127.0.0.1";

            //Act
            string port = portService.GenerateClientMessagingPort(clientId, ipAddress);

            //Assert
            Assert.NotNull(port);
            Assert.InRange(Int32.Parse(port), 57344, 65535);
        }

        [Fact]
        public void testGettingClientsMessagingEndpoint()
        {
            //Arrange
            PortService portService = new PortService();
            string clientId = "test";
            string ipAddress = "127.0.0.1";
            string port = portService.GenerateClientMessagingPort(clientId, ipAddress);
            string expectedEndpoint = $"{ipAddress}:{port}";

            //Act
            string endpoint = portService.GetClientMessagingEndpoint(clientId);

            //Assert
            Assert.NotNull(endpoint);
            Assert.Equal(expectedEndpoint, endpoint);
        }

        [Fact]
        public void testUnassigningClientsRequestPort()
        {
            //Arrange
            PortService portService = new PortService();
            string clientId = "test";
            string ipAddress = "127.0.0.1";
            string port = portService.GenerateClientRequestPort(clientId, ipAddress);

            //Act
            bool portUnassigned = portService.UnassignClientsRequestPort(clientId);

            //Assert
            Assert.True(portUnassigned);
        }

        [Fact]
        public void testUnassigningClientsMessagePort()
        {
            //Arrange
            PortService portService = new PortService();
            string clientId = "test";
            string ipAddress = "127.0.0.1";
            string port = portService.GenerateClientMessagingPort(clientId, ipAddress);

            //Act
            bool portUnassigned = portService.UnassignClientsMessagingPort(clientId);

            //Assert
            Assert.True(portUnassigned);
        }
    }
}
