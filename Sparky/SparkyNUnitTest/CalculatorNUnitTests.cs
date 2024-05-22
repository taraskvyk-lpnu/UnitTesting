using NUnit.Framework;
using Sparky;

namespace SparkyNUnitTest
{
    [TestFixture]
    public class CalculatorNUnit
    {
        [Test] 
        public void AddNumbers_InputTwoInt_ReturnsCorrectAddition()
        {
            //Arrangr
            Calculator calc = new();

            //Act
            int result = calc.AddNumbers(10, 20);

            //Assert
            Assert.That(result, Is.EqualTo(30));
        }

        [Test]
        [TestCase(10)]
        [TestCase(12)]
        public void IsOdd_EvenNumberPassed_ReturnsFalse(int a)
        {
            //Arrangr
            Calculator calc = new();

            //Act
            bool result = calc.IsOddNumber(a);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsOdd_OddNumberPassed_ReturnsTrue()
        {
            //Arrangr
            Calculator calc = new();

            //Act
            bool result = calc.IsOddNumber(11);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase(11, ExpectedResult = true)]
        [TestCase(10, ExpectedResult = false)]
        [TestCase(11, ExpectedResult = true)]
        [TestCase(12, ExpectedResult = false)]
        public bool IsOdd_NumberPassed_ReturnsTrueIfOdd(int a)
        {
            //Arrangr
            Calculator calc = new();

            //Act & Assert
            return calc.IsOddNumber(a);
        }

        [Test]
        [TestCase(5.4, 10.5)]       // 15.9
        [TestCase(5.43, 10.56)]     // 15.96
        [TestCase(5.49, 10.59)]     //16.08
        public void AddNumbersDouble_InputTwoDouble_ReturnsCorrectAddition(double a, double b)
        {
            //Arrangr
            Calculator calc = new();

            //Act
            double result = calc.AddNumbersDouble(a, b);

            //Assert
            Assert.That(result, Is.EqualTo(15.9).Within(0.2));
        }

        [Test]
        public void OddRange_InputMinMaxRange_ReturnsValidOddNumbersRange()
        {
            //Arrange
            Calculator calculator = new();
            List<int> expectedOddRange = new List<int>() { 5, 7, 9 };

            //Act
            List<int> result = calculator.GetOddRange(5, 10);

            //Assert
            Assert.That(result, Is.EquivalentTo(expectedOddRange));
            Assert.That(result, Does.Contain(7));
            Assert.That(result, Has.No.Member(6));
            Assert.That(result.Count, Is.EqualTo(3));

            Assert.That(result, Is.Ordered);
            Assert.That(result, Is.Unique);
        }
    }
}
