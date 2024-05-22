using NUnit.Framework;
using Sparky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SparkyNUnitTest
{
    [TestFixture]
    public class FiboNUnitTests
    {
        Fibo fibo;

        [SetUp]
        public void SetUp()
        {
            fibo = new Fibo();
        }

        [Test]
        public void GetFiboSeries_InputRange1_ReturnsValidCollection()
        {
            int range = 1;

            fibo.Range = range;
            List<int> fiboRange = fibo.GetFiboSeries();

            Assert.That(fiboRange, Does.Not.Empty);
            Assert.That(fiboRange, Is.Ordered);
            Assert.That(fiboRange, Does.Contain(0));
        }


        [Test]
        public void GetFiboSeries_InputRange6_ReturnsValidCollection()
        {
            int range = 6;

            fibo.Range = range;
            List<int> fiboRange = fibo.GetFiboSeries();

            Assert.That(fiboRange, Does.Contain(3));
            Assert.That(fiboRange.Count, Is.EqualTo(6));
            Assert.That(fiboRange, Has.No.Member(4));

            Assert.That(fiboRange, Is.EquivalentTo(new List<int> { 0, 1, 1, 2, 3, 5}));
        }
    }
}
