using Moq;
using Sparky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkyXUnitTest
{
    public class ProductXUnitTests
    {
        [Fact]
        public void GetProductPrice_PlatinumCustomer_ReturnsPriceWith20PersDiscount()
        {
            Product product = new Product() { Price = 50 };

            var result = product.GetPrice(new Customer { IsPlatinum = true });

            Assert.Equal(40, result);
        }

        // Не добрий підхід, якщо штучно створювати інтерфейс для класу лише заради тестування
        [Fact]
        public void GetProductPrice_MOQAbusePlatinumCustomer_ReturnsPriceWith20PersDiscount()
        {
            Product product = new Product() { Price = 50 };

            var customerMock = new Mock<ICustomer>();
            customerMock.Setup(x => x.IsPlatinum).Returns(true);

            var result = product.GetPrice(customerMock.Object);

            Assert.Equal(40, result);
        }
    }
}
