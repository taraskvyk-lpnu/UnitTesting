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
    public class BankAccountNUnitTests
    {
        BankAccount bankAccount;

        // Не Unit тест а Integration тест, бо взаємодія кількох об'єктів класів
        [SetUp]
        public void SetUp()
        {

        }

        // Не Unit тест а Integration тест, бо взаємодія кількох об'єктів класів
        [Test]
        public void BankDeposit_Add100LogBook_ReturnsTrue()
        {
            BankAccount account = new(new LogBook());
            // LogFaker нічого не робить, тому це вже Unit тест
            var result = account.Deposit(100);

            Assert.That(result, Is.True);
            Assert.That(account.GetCurrentBalance(), Is.EqualTo(100));
        }

        // Не Unit тест а Integration тест, бо взаємодія кількох об'єктів класів
        [Test]
        public void BankDeposit_Add100Moq_ReturnsTrue()
        {
            var logMock = new Mock<ILogBook>();

            BankAccount account = new(logMock.Object);
            // LogFaker нічого не робить, тому це вже Unit тест
            var result = account.Deposit(100);

            Assert.That(result, Is.True);
            Assert.That(account.GetCurrentBalance(), Is.EqualTo(100));
        }

        [Test]
        [TestCase(200, 100)]
        public void BankWithdraw_Withdraw100With200Balance_ReturnsTrue(int balance, int withdrawAmount)
        {
            var logMock = new Mock<ILogBook>();
            logMock.Setup(u => u.LogToDb(It.IsAny<string>())).Returns(true);

            logMock.Setup(u => u.LogBalanceAfterWithdraw(It.Is<int>(x => x >= 0))).Returns(true);
            
            BankAccount account = new(logMock.Object);
            account.Deposit(balance);
            var result = account.Withdraw(withdrawAmount);

            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase(200, 300)]
        public void BankWithdraw_Withdraw300With200Balance_ReturnsFalse(int balance, int withdrawAmount)
        {
            var logMock = new Mock<ILogBook>();
            logMock.Setup(u => u.LogToDb(It.IsAny<string>())).Returns(true);

            logMock.Setup(u => u.LogBalanceAfterWithdraw(It.Is<int>(x => x >= 0))).Returns(true);
            //logMock.Setup(u => u.LogBalanceAfterWithdraw(It.Is<int>(x => x < 0))).Returns(false);

            logMock.Setup(u => u.LogBalanceAfterWithdraw(It.IsInRange(int.MinValue, -1, Moq.Range.Inclusive))).Returns(false);

            BankAccount account = new(logMock.Object);
            account.Deposit(balance);
            var result = account.Withdraw(withdrawAmount);

            Assert.That(result, Is.False);
        }

        [Test]
        public void BankLog_LogMockString_ReturnsTrue()
        {
            var logMock = new Mock<ILogBook>();
            string desiredOutput = "hello";
            
            logMock.Setup(u => u.MessageWithReturnStr(It.IsAny<string>())).Returns((string str) => str.ToLower());
            Assert.That(logMock.Object.MessageWithReturnStr("HelLO"), Is.EqualTo(desiredOutput));
        }

        [Test]
        public void BankLogTaras_LogMockStringOutputStr_ReturnsTrue()
        {
            var logMock = new Mock<ILogBook>();
            string desiredOutput = "Hello";

            logMock.Setup(u => u.LogWithOutputResult(It.IsAny<string>(), out desiredOutput)).Returns(true);

            string result1 = string.Empty;
            Assert.That(logMock.Object.LogWithOutputResult("Taras", out result1), Is.True);
            Assert.That(result1, Is.EqualTo(desiredOutput));
        }

        [Test]
        public void BankLogTaras_LogRefChecker_ReturnsTrue()
        {
            var logMock = new Mock<ILogBook>();
            Customer customer = new();
            Customer customerNotUsed = new();

            logMock.Setup(u => u.LogWithRefObj(ref customer)).Returns(true);

            Assert.That(logMock.Object.LogWithRefObj(ref customer), Is.True);
            Assert.That(logMock.Object.LogWithRefObj(ref customerNotUsed), Is.False);
        }

        [Test]
        public void Properties_SetAndGetLogTypleAndLogSeverity_MockTest()
        {
            var logMock = new Mock<ILogBook>();
            logMock.Setup(u => u.LogSeverity).Returns(10);
            
            logMock.SetupAllProperties();
            logMock.Setup(u => u.LogType).Returns("Warning");
            logMock.Object.LogSeverity = 100;

            Assert.That(logMock.Object.LogSeverity, Is.EqualTo(100));
            Assert.That(logMock.Object.LogType, Is.EqualTo("Warning"));

            //Callbacks
            string logTemp = "Hello, ";
            logMock.Setup(u => u.LogToDb(It.IsAny<string>()))
                .Returns(true).Callback((string str) => logTemp += str);

            logMock.Object.LogToDb("Taras");

            Assert.That(logTemp, Is.EqualTo("Hello, Taras"));
        }

        [Test]
        public void Callbacks_MockTest()
        {
            var logMock = new Mock<ILogBook>();
            logMock.Setup(u => u.LogSeverity).Returns(10);

            //Callbacks
            string logTemp = "Hello, ";
            logMock.Setup(u => u.LogToDb(It.IsAny<string>()))
                .Returns(true).Callback((string str) => logTemp += str); // str - переданий параметр

            logMock.Object.LogToDb("Taras");
            Assert.That(logTemp, Is.EqualTo("Hello, Taras"));

            int counter = 5;
            logMock.Setup(u => u.LogToDb(It.IsAny<string>()))
                .Callback(() => counter++).Returns(true).Callback(() => counter++); // str - переданий параметр

            logMock.Object.LogToDb("Taras");
            Assert.That(counter, Is.EqualTo(7));
        }

        [Test]
        public void BankLog_VerifyExample()
        {
            var logMock = new Mock<ILogBook>();
            BankAccount account = new(logMock.Object);
            account.Deposit(100);
            Assert.That(account.GetCurrentBalance(), Is.EqualTo(100));

            // Verification
            logMock.Verify(u => u.Message(It.IsAny<string>()), Times.Exactly(2));
            logMock.Verify(u => u.Message("Test"), Times.Once);
            logMock.VerifySet(u => u.LogSeverity = 101, Times.Once);
            logMock.VerifyGet(u => u.LogSeverity, Times.Never);
        }
    }
}
