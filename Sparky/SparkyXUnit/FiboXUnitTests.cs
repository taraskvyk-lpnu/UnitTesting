using Sparky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SparkyXUnitTest
{
    public class FiboXUnitTests
    {
        Fibo fibo;

        public FiboXUnitTests()
        {
            fibo = new Fibo();
        }

        [Fact]
        public void GetFiboSeries_InputRange1_ReturnsValidCollection()
        {
            int range = 1;

            fibo.Range = range;
            List<int> fiboRange = fibo.GetFiboSeries();

            Assert.NotEmpty(fiboRange);
            Assert.Equal(fiboRange.OrderBy(u => u), fiboRange);
            Assert.Contains(0, fiboRange);
        }

        [Fact]
        public void GetFiboSeries_InputRange6_ReturnsValidCollection()
        {
            int range = 6;

            fibo.Range = range;
            List<int> fiboRange = fibo.GetFiboSeries();

            Assert.Contains(3, fiboRange);
            Assert.Equal(6, fiboRange.Count);

            Assert.DoesNotContain(4, fiboRange);

            Assert.Equal(fiboRange, new List<int> { 0, 1, 1, 2, 3, 5 });
        }
    }
}
