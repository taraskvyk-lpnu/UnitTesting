using Sparky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SparkyXUnitTest
{
    public class CustomerXUnitTests
    {
        private Customer customer;

        public CustomerXUnitTests()
        {
            customer = new Customer();
        }

        [Fact]
        public void CombineNames_InputFirstAndLastName_ReturnsFullName()
        {
            customer.GreetAndCombineNames("Taras", "Kvyk");
            Assert.Multiple(() =>
            {
                Assert.Equal("Hello, Taras Kvyk", customer.GreetMessage);
                Assert.Contains("Taras Kvyk", customer.GreetMessage)
                ;
                Assert.StartsWith("Hello,", customer.GreetMessage);
                Assert.EndsWith("Kvyk", customer.GreetMessage);
                Assert.Matches("Hello, [A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", customer.GreetMessage); // Regular expressions
                Assert.Contains("Hello, Taras Kvyk", customer.GreetMessage);
            });
        }

        [Fact]
        public void GreetMessage_NotGreeted_ReturnsNull()
        {
            //Arrenge
            //Act
            //Assert
            Assert.Null(customer.GreetMessage);
        }

        [Fact]
        public void DiscountCheck_DefauletUser_ReturnsDiscountInRange()
        {
            int result = customer.Discount;

            Assert.InRange(result, 10, 20);
        }

        [Fact]
        public void GreetMessage_InputWithoutLastName_ReturnsNotNull()
        {
            customer.GreetAndCombineNames("Taras", "");

            Assert.NotNull(customer.GreetMessage);
        }

        [Fact]
        public void GreetMessage_InputWithoutFirstName_ThrowsException()
        {
            var exceptionDetails = Assert.Throws<ArgumentException>(
                () => customer.GreetAndCombineNames("", "Kvyk"));

            Assert.Equal("Empty First Name", exceptionDetails.Message);

            //Assert.Throws(() => customer.GreetAndCombineNames("", "Kvyk"),
            //    Throws.ArgumentException.With.Message.EqualTo("Empty First Name"));

            Assert.Throws<ArgumentException>(
                () => customer.GreetAndCombineNames("", "Kvyk"));

            //Assert.That(exceptionDetails.Message, Is.EqualTo("Empty First Name"));
            //Assert.That(() => customer.GreetAndCombineNames("", "Kvyk"),
            //    Throws.ArgumentException);
        }

        [Fact]
        public void GetCustomerDetails_InputOrderTotalLess100_ReturnsBasicCustomer()
        {
            customer.OrderTotal = 10;

            var result = customer.GetCustomerDetails();

            Assert.IsType<BasicCustomer>(result);
        }

        [Fact]
        public void GetCustomerDetails_InputOrderTotalGreater100_ReturnsPlatinumCustomer()
        {
            customer.OrderTotal = 300;

            var result = customer.GetCustomerDetails();

            Assert.IsType<PlatinumCustomer>(result);
        }
    }
}