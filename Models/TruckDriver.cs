using swiftroute_courier_app.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class TruckDriver : Courier
    {
        public TruckDriver(string name, string phone)
            : base(name, phone)
        {
        }

        public override bool CanAccept(Shipment shipment)
        {
            //bool economyOnly = shipment.ServiceTier == ServiceTier.Economy;

            bool tireAllowed = shipment.ServiceTier == ServiceTier.Economy || shipment.ServiceTier == ServiceTier.Vip;

            bool minimumCargo = shipment.Parcel.WeightKg >= 20m;

            return tireAllowed && minimumCargo;
        }

        public bool CanDispatch(decimal totalCargoWeight)
        {
            return totalCargoWeight >= 20m;
        }
    }
}
