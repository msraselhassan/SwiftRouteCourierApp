using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class CashOnDelivery
    {
        public decimal Amount { get; }
        public decimal ServiceFeeRate { get; } = 0.01m;

        public decimal ServiceFee => CalculateServiceFee();
        public decimal RemittanceAmount => CalculateRemittanceAmount();

        public CashOnDelivery(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("COD amount must be greater than zero.");
            }

            Amount = amount;
        }

        public decimal CalculateServiceFee()
        {
            return Amount * ServiceFeeRate;
        }

        public decimal CalculateRemittanceAmount()
        {
            return Amount - ServiceFee;
        }
    }
}
