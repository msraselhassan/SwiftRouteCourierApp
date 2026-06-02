using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class Contact
    {
        public string Name { get; }
        public string Phone { get; }
        public Address Address { get; }

        public Contact(string name, string phone, Address address)
        {
            Name = name;
            Phone = phone;
            Address = address;
        }

        public override string ToString()
        {
            return $"{Name}, {Phone}, {Address}";
        }
    }
}
