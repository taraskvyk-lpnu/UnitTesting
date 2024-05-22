using Bongo.Models.ModelValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bongo.Models
{
    [TestFixture]
    public class DateInFutureAttributeTests
    {
        [Test]
        [TestCase(100, ExpectedResult = true)]
        [TestCase(-100, ExpectedResult = false)]
        [TestCase(0, ExpectedResult = false)]
        public bool DateValidator_InputExpectedDateRange_DateValidity(int secondsToAdd)
        {
            DateInFutureAttribute dateInFutureAttribute = new DateInFutureAttribute(() => DateTime.Now);

            return dateInFutureAttribute.IsValid(DateTime.Now.AddSeconds(secondsToAdd));
        }

        [Test]
        public void DateValidator_NotValidDate_ReturnsErrorMessage()
        {
            var dateInFutureAttribute = new DateInFutureAttribute();

            Assert.AreEqual("Date must be in the future", dateInFutureAttribute.ErrorMessage);
        }
    }
}
