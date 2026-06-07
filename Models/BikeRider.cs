using swiftroute_courier_app.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class BikeRider : Courier
    {
        public BikeRider(string name, string phone)
            : base(name, phone)
        {
        }

        public override bool CanAccept(Shipment shipment)
        {
            if (shipment.ServiceTier == ServiceTier.Vip)
            {
                return false;
            }

            bool isRefridgerated = shipment.Parcel is RefrigeratedParcel;
            bool weightAllowed = shipment.Parcel.WeightKg <= 5m;

            bool tierAllowed =
                shipment.ServiceTier == ServiceTier.SameDay ||
                shipment.ServiceTier == ServiceTier.NextDay;

            return !isRefridgerated && weightAllowed && tierAllowed;
        }
    }
}
