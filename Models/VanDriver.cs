using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class VanDriver : Courier
    {
        public VanDriver(string name, string phone)
            : base(name, phone)
        {
        }

        public override bool CanAccept(Shipment shipment)
        {
            return shipment.Parcel.WeightKg <= 50m;
        }
    }
}
