using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class RefrigeratedParcel : Parcel
    {
        public RefrigeratedParcel(Contact sender, Contact recipient, decimal weightKg, decimal? declaredValue) : base(sender, recipient, weightKg, declaredValue)
        {
            Validate();
        }

        public override bool CanBeInsured => true;

        public override bool RequiresInsurance => false;

        public override bool AllowsCod => true;

        public override decimal GetSpecialSurcharge()
        {
           return  100m;
        }

        public override decimal GetWeightSurcharge()
        {
            return CalculateStandardWeightSurcharge();
        }

        public override void Validate()
        {
            if (WeightKg < 0.1m || WeightKg > 20m)
            {
                throw new InvalidOperationException("Refrigerated parcel must be 0.1 kg to 20 kg.");
            }
        }
    }
}
