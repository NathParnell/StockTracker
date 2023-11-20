using StockTrackerServer.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerServerTests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IDataService> _dataServiceMock = new Mock<IDataService>();

        private readonly AuthenticationService _sut;

        private readonly string _email = "testUser@test.com";
        private readonly string _password = "testPassword";

        public AuthenticationServiceTests()
        {
            _sut = new AuthenticationService(_dataServiceMock.Object);
        }


        [Fact]
        public void testAuthenticatingSupplierWhenSupplierExists()
        {
            //Arrange
            _dataServiceMock.Setup(x => x.GetSupplierByEmail(_email.ToLower()))
                .ReturnsAsync(new Supplier { Email = _email, Password = _password });

            //Act
            var result = _sut.AuthenticateSupplier(_email, _password);

            //Assert
            //Check that the result is not null
            Assert.NotNull(result);
            //Check that the result is of type Supplier
            Assert.IsType<Supplier>(result);
            //Check that the result is the same as the supplier we set up
            Assert.Equal(_email, result.Email);
            Assert.Equal(_password, result.Password);
        }

        [Fact]
        public void testAuthenticatingSupplierWhenSupplierDoesNotExist()
        {
            //Arrange
            _dataServiceMock.Setup(x => x.GetSupplierByEmail(_email.ToLower()))
                .ReturnsAsync((Supplier?)null);

            //Act
            var result = _sut.AuthenticateSupplier(_email, _password);

            //Assert
            //Check that the result is null
            Assert.Null(result);
        }

        [Fact]
        public void testAuthenticatingSupplierWhenSupplierExistsButPasswordIsWrong()
        {
            //Arrange
            _dataServiceMock.Setup(x => x.GetSupplierByEmail(_email.ToLower()))
                .ReturnsAsync(new Supplier { Email = _email, Password = _password });

            //Act
            var result = _sut.AuthenticateSupplier(_email, "wrongPassword");

            //Assert
            //Check that the result is null
            Assert.Null(result);
        }

        [Fact]
        public void testAuthenticatingCustomerWhenCustomerExists()
        {
            //Arrange
            _dataServiceMock.Setup(x => x.GetCustomerByEmail(_email.ToLower()))
                .ReturnsAsync(new Customer { Email = _email, Password = _password });

            //Act
            var result = _sut.AuthenticateCustomer(_email, _password);

            //Assert
            //Check that the result is not null
            Assert.NotNull(result);
            //Check that the result is of type Customer
            Assert.IsType<Customer>(result);
            //Check that the result is the same as the customer we set up
            Assert.Equal(_email, result.Email);
            Assert.Equal(_password, result.Password);
        }

        [Fact]
        public void testAuthenticatingCustomerWhenCustomerDoesNotExist()
        {
            //Arrange
            _dataServiceMock.Setup(x => x.GetCustomerByEmail(_email.ToLower()))
                .ReturnsAsync((Customer?)null);

            //Act
            var result = _sut.AuthenticateCustomer(_email, _password);

            //Assert
            //Check that the result is null
            Assert.Null(result);
        }

        [Fact]
        public void testAuthenticatingCustomerWhenCustomerExistsButPasswordIsWrong()
        {
            //Arrange
            _dataServiceMock.Setup(x => x.GetCustomerByEmail(_email.ToLower()))
                .ReturnsAsync(new Customer { Email = _email, Password = _password });

            //Act
            var result = _sut.AuthenticateCustomer(_email, "wrongPassword");

            //Assert
            //Check that the result is null
            Assert.Null(result);
        }
    }
}
