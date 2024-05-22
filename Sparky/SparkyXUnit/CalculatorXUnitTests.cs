using Sparky;

namespace SparkyXUnitTest
{
    public class CalculatorXUnit
    {
        [Fact]
        public void AddNumbers_InputTwoInt_ReturnsCorrectAddition()
        {
            //Arrangr
            Calculator calc = new();

            //Act
            int result = calc.AddNumbers(10, 20);

            //Assert
            Assert.Equal(result, 30);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(12)]
        public void IsOdd_EvenNumberPassed_ReturnsFalse(int a)
        {
            //Arrangr
            Calculator calc = new();

            //Act
            bool result = calc.IsOddNumber(a);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void IsOdd_OddNumberPassed_ReturnsTrue()
        {
            //Arrangr
            Calculator calc = new();

            //Act
            bool result = calc.IsOddNumber(11);

            //Assert
            Assert.True(result);
        }


        [Theory]
        [InlineData(11, true)]
        [InlineData(10, false)]
        [InlineData(11, true)]
        [InlineData(12, false)]
        public void IsOdd_NumberPassed_ReturnsTrueIfOdd(int a, bool expectedResult)
        {
            //Arrangr
            Calculator calc = new();

            //Act & Assert
            Assert.Equal(expectedResult, calc.IsOddNumber(a));
        }


        [Theory]
        [InlineData(5.4, 10.5)]
        [InlineData(5.43, 10.56)]
        [InlineData(5.49, 10.59)]
        public void AddNumbersDouble_InputTwoDouble_ReturnsCorrectAddition(double a, double b)
        {
            //Arrangr
            Calculator calc = new();

            //Act
            double result = calc.AddNumbersDouble(a, b);

            //Assert
            Assert.Equal(15.9, result, 0.2);
        }

        [Fact]
        public void OddRange_InputMinMaxRange_ReturnsValidOddNumbersRange()
        {
            //Arrange
            Calculator calculator = new();
            List<int> expectedOddRange = new List<int>() { 5, 7, 9 };

            //Act
            List<int> result = calculator.GetOddRange(5, 10);

            //Assert
            Assert.Equal(expectedOddRange, result);

            Assert.Contains(7, result);
            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count);

            Assert.DoesNotContain(6, result);
            Assert.Equal(result.OrderBy(u => u), result);

            //Assert.That(result, Is.Unique);
        }
    }
}
