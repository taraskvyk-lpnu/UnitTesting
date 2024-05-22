using Sparky;

namespace CalculatorMSTests
{
    [TestClass]
    public class CalculatlorMSTests
    {
        [TestMethod]
        public void AddNumbers_InputTwoInt_ReturnsCorrectAddition()
        {
            //Arrangr
            Calculator calc = new();

            //Act
            int result = calc.AddNumbers(10, 20);

            //Assert
            Assert.AreEqual(30, result);
        }
    }
}