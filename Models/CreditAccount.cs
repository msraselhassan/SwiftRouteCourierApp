using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class CreditAccount
    {
        public string AccountNumber { get; }
        public decimal Balance { get; private set; }
        public decimal CreditLimit { get; }

        public CreditAccount(string accountNumber, decimal creditLimit)
        {
            AccountNumber = accountNumber;
            CreditLimit = creditLimit;
            Balance = 0m;
        }

        public void AddCharge(decimal amount)
        {
            if (Balance + amount > CreditLimit)
            {
                throw new InvalidOperationException("Credit limit exceeded.");
            }

            Balance += amount;
        }

        public void ClearBalance()
        {
            Balance = 0m;
        }
    }
}
