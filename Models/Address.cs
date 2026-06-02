using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class Address
    {
        public string House { get; }
        public string Road { get; }
        public string Area { get; }
        public string City { get; }
        public string District { get; }

        public Address(string house, string road, string area, string city, string district)
        {
            House = house;
            Road = road;
            Area = area;
            City = city;
            District = district;
        }

        public override string ToString()
        {
            return $"{House}, {Road}, {Area}, {City}, {District}";
        }
    }
}
