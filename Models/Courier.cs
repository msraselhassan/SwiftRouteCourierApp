using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public abstract class Courier
    {
        public Guid CourierId { get; } = Guid.NewGuid();
        public string Name { get; }
        public string Phone { get; }

        protected Courier(string name, string phone)
        {
            Name = name;
            Phone = phone;
        }

        public abstract bool CanAccept(Shipment shipment);

        public override string ToString()
        {
            return $"{Name} ({GetType().Name})";
        }
    }
}
