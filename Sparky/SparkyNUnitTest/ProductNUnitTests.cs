using Moq;
using NUnit.Framework;
using Sparky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkyNUnitTest
{
    [TestFixture]
    public class ProductNUnitTests
    {
        [Test]
        public void GetProductPrice_PlatinumCustomer_ReturnsPriceWith20PersDiscount()
        {
            Product product = new Product() { Price = 50 };

            var result = product.GetPrice(new Customer { IsPlatinum = true });

            Assert.That(result, Is.EqualTo(40));
        }

        // Не добрий підхід, якщо штучно створювати інтерфейс для класу лише заради тестування
        [Test]
        public void GetProductPrice_MOQAbusePlatinumCustomer_ReturnsPriceWith20PersDiscount()
        {
            Product product = new Product() { Price = 50 };

            var customerMock = new Mock<ICustomer>();
            customerMock.Setup(x => x.IsPlatinum).Returns(true);

            var result = product.GetPrice(customerMock.Object);

            Assert.That(result, Is.EqualTo(40));
        }
    }
}
