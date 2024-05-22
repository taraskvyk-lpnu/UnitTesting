using Moq;
using Sparky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkyXUnitTest
{
    public class BankAccountXUnitTests
    {
        // Не Unit тест а Integration тест, бо взаємодія кількох об'єктів класів
        [Fact]
        public void BankDeposit_Add100LogBook_ReturnsTrue()
        {
            BankAccount account = new(new LogBook());
            // LogFaker нічого не робить, тому це вже Unit тест
            var result = account.Deposit(100);

            Assert.True(result);
            Assert.Equal(100, account.GetCurrentBalance());
        }

        // Не Unit тест а Integration тест, бо взаємодія кількох об'єктів класів
        [Fact]
        public void BankDeposit_Add100Moq_ReturnsTrue()
        {
            var logMock = new Mock<ILogBook>();

            BankAccount account = new(logMock.Object);
            // LogFaker нічого не робить, тому це вже Unit тест
            var result = account.Deposit(100);

            Assert.True(result);
            Assert.Equal(100, account.GetCurrentBalance());
        }

        [Theory]
        [InlineData(200, 100)]
        public void BankWithdraw_Withdraw100With200Balance_ReturnsTrue(int balance, int withdrawAmount)
        {
            var logMock = new Mock<ILogBook>();
            logMock.Setup(u => u.LogToDb(It.IsAny<string>())).Returns(true);

            logMock.Setup(u => u.LogBalanceAfterWithdraw(It.Is<int>(x => x >= 0))).Returns(true);

            BankAccount account = new(logMock.Object);
            account.Deposit(balance);
            var result = account.Withdraw(withdrawAmount);

            Assert.True(result);
        }

        [Theory]
        [InlineData(200, 300)]
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

            Assert.False(result);
        }

        [Fact]
        public void BankLog_LogMockString_ReturnsTrue()
        {
            var logMock = new Mock<ILogBook>();
            string desiredOutput = "hello";

            logMock.Setup(u => u.MessageWithReturnStr(It.IsAny<string>())).Returns((string str) => str.ToLower());
            Assert.Equal(desiredOutput, logMock.Object.MessageWithReturnStr("HelLO"));
        }

        [Fact]
        public void BankLogTaras_LogMockStringOutputStr_ReturnsTrue()
        {
            var logMock = new Mock<ILogBook>();
            string desiredOutput = "Hello";

            logMock.Setup(u => u.LogWithOutputResult(It.IsAny<string>(), out desiredOutput)).Returns(true);

            string result1 = string.Empty;
            Assert.True(logMock.Object.LogWithOutputResult("Taras", out result1));
            Assert.Equal(desiredOutput, result1);
        }

        [Fact]
        public void BankLogTaras_LogRefChecker_ReturnsTrue()
        {
            var logMock = new Mock<ILogBook>();
            Customer customer = new();
            Customer customerNotUsed = new();

            logMock.Setup(u => u.LogWithRefObj(ref customer)).Returns(true);

            Assert.True(logMock.Object.LogWithRefObj(ref customer));
            Assert.False(logMock.Object.LogWithRefObj(ref customerNotUsed));
        }

        [Fact]
        public void Properties_SetAndGetLogTypleAndLogSeverity_MockTest()
        {
            var logMock = new Mock<ILogBook>();
            logMock.Setup(u => u.LogSeverity).Returns(10);

            logMock.SetupAllProperties();
            logMock.Setup(u => u.LogType).Returns("Warning");
            logMock.Object.LogSeverity = 100;

            Assert.Equal(100, logMock.Object.LogSeverity);
            Assert.Equal("Warning", logMock.Object.LogType);

            //Callbacks
            string logTemp = "Hello, ";
            logMock.Setup(u => u.LogToDb(It.IsAny<string>()))
                .Returns(true).Callback((string str) => logTemp += str);

            logMock.Object.LogToDb("Taras");

            Assert.Equal("Hello, Taras", logTemp);
        }

        [Fact]
        public void Callbacks_MockTest()
        {
            var logMock = new Mock<ILogBook>();
            logMock.Setup(u => u.LogSeverity).Returns(10);

            //Callbacks
            string logTemp = "Hello, ";
            logMock.Setup(u => u.LogToDb(It.IsAny<string>()))
                .Returns(true).Callback((string str) => logTemp += str); // str - переданий параметр

            logMock.Object.LogToDb("Taras");
            Assert.Equal("Hello, Taras", logTemp);

            int counter = 5;
            logMock.Setup(u => u.LogToDb(It.IsAny<string>()))
                .Callback(() => counter++).Returns(true).Callback(() => counter++); // str - переданий параметр

            logMock.Object.LogToDb("Taras");
            Assert.Equal(7, counter);
        }

        [Fact]
        public void BankLog_VerifyExample()
        {
            var logMock = new Mock<ILogBook>();
            BankAccount account = new(logMock.Object);
            account.Deposit(100);
            Assert.Equal(100, account.GetCurrentBalance());

            // Verification
            logMock.Verify(u => u.Message(It.IsAny<string>()), Times.Exactly(2));
            logMock.Verify(u => u.Message("Test"), Times.Once);
            logMock.VerifySet(u => u.LogSeverity = 101, Times.Once);
            logMock.VerifyGet(u => u.LogSeverity, Times.Never);
        }
    }
}
