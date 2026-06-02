using swiftroute_courier_app.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public abstract class Customer
    {
        public Guid CustomerId { get; } = Guid.NewGuid();
        public string Name { get; }
        public string Phone { get; }

        public abstract decimal DiscountRate { get; }
        public abstract bool CanUseCod { get; }
        public abstract PaymentMode PaymentMode { get; }

        protected Customer(string name, string phone)
        {
            Name = name;
            Phone = phone;
        }

        public override string ToString()
        {
            return $"{Name} ({GetType().Name})";
        }
    }
}
