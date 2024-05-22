using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparky
{
    public class BankAccount
    {
        public int balance { get; set; }
        private ILogBook _logger;

        public BankAccount(ILogBook logger)
        {
            balance = 0;
            _logger = logger;
        }

        public bool Deposit(int amount)
        {
            _logger.Message("Deposit invoked");
            _logger.Message("Test");
            _logger.LogSeverity = 101;

            balance += amount;
            return true;
        }

        public bool Withdraw(int amount)
        {
            _logger.Message("Withdraw invoked");
            
            if (amount <= balance)
            {
                _logger.LogToDb("Withdrawal Amount: " + amount.ToString());
                balance -= amount;
               
                return _logger.LogBalanceAfterWithdraw(balance);
            }

            return _logger.LogBalanceAfterWithdraw(balance - amount);
        }

        public int GetCurrentBalance() => balance; 

    }
}
