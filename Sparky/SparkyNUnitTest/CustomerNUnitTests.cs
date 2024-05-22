using NUnit.Framework;
using NUnit.Framework.Legacy;
using Sparky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SparkyNUnitTest
{
    [TestFixture]
    public class CustomerNUnitTests
    {
        private Customer customer;
        [SetUp]
        public void SetUp()
        {
            customer = new Customer();
        }

        [Test]
        public void CombineNames_InputFirstAndLastName_ReturnsFullName()
        {   
            customer.GreetAndCombineNames("Taras", "Kvyk");
            Assert.Multiple(() =>
            {
                Assert.That(customer.GreetMessage, Is.EqualTo("Hello, Taras Kvyk"));
                Assert.That(customer.GreetMessage, Does.Contain("Taras Kvyk"));
                Assert.That(customer.GreetMessage, Does.StartWith("Hello,"));
                Assert.That(customer.GreetMessage, Does.EndWith("Kvyk"));
                Assert.That(customer.GreetMessage, Does.Match("Hello, [A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+")); // Regular expressions
                StringAssert.Contains(customer.GreetMessage, "Hello, Taras Kvyk");
            });
        }

        [Test]
        public void GreetMessage_NotGreeted_ReturnsNull()
        {
            //Arrenge
            //Act
            //Assert
            Assert.That(customer.GreetMessage, Is.Null);
        }

        [Test]
        public void DiscountCheck_DefauletUser_ReturnsDiscountInRange()
        {
            int result = customer.Discount;

            Assert.That(result, Is.InRange(10, 20));  
        }

        [Test]
        public void GreetMessage_InputWithoutLastName_ReturnsNotNull()
        {
            customer.GreetAndCombineNames("Taras", "");

            Assert.That(customer.GreetMessage, Is.Not.Null);
        }

        [Test]
        public void GreetMessage_InputWithoutFirstName_ThrowsException()
        {
            var exceptionDetails = Assert.Throws<ArgumentException>(
                () => customer.GreetAndCombineNames("", "Kvyk"));
            
            Assert.That(exceptionDetails.Message, Is.EqualTo("Empty First Name"));
            Assert.That(() => customer.GreetAndCombineNames("", "Kvyk"), 
                Throws.ArgumentException.With.Message.EqualTo("Empty First Name"));

            Assert.Throws<ArgumentException>(
                () => customer.GreetAndCombineNames("", "Kvyk"));

            Assert.That(exceptionDetails.Message, Is.EqualTo("Empty First Name"));
            Assert.That(() => customer.GreetAndCombineNames("", "Kvyk"),
                Throws.ArgumentException);
        }

        [Test]
        public void GetCustomerDetails_InputOrderTotalLess100_ReturnsBasicCustomer()
        {
            customer.OrderTotal = 10;

            var result = customer.GetCustomerDetails();

            Assert.That(result, Is.TypeOf<BasicCustomer>());
        }

        [Test]
        public void GetCustomerDetails_InputOrderTotalGreater100_ReturnsPlatinumCustomer()
        {
            customer.OrderTotal = 300;

            var result = customer.GetCustomerDetails();

            Assert.That(result, Is.TypeOf<PlatinumCustomer>());
        }
    }
}