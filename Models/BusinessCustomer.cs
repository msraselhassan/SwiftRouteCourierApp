using swiftroute_courier_app.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class BusinessCustomer : Customer
    {
        public string CompanyName { get; }
        public CreditAccount CreditAccount { get; }

        public override decimal DiscountRate => 0.10m;
        public override bool CanUseCod => false;
        public override PaymentMode PaymentMode => PaymentMode.MonthlyCredit;

        public BusinessCustomer(string name, string phone, string companyName, CreditAccount creditAccount)
            : base(name, phone)
        {
            CompanyName = companyName;
            CreditAccount = creditAccount;
        }

        public override string ToString()
        {
            return $"{CompanyName} ({Name})";
        }
    }
}
