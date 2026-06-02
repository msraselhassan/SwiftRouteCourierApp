using swiftroute_courier_app.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class IndividualCustomer : Customer
    {
        public override decimal DiscountRate => 0m;
        public override bool CanUseCod => true;
        public override PaymentMode PaymentMode => PaymentMode.CashAtPickup;

        public IndividualCustomer(string name, string phone)
            : base(name, phone)
        {
        }
    }
}
