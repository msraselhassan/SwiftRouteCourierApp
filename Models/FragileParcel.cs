using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class FragileParcel : Parcel
    {
        public override bool CanBeInsured => true;
        public override bool RequiresInsurance => true;
        public override bool AllowsCod => true;

        public FragileParcel(Contact sender, Contact recipient, decimal weightKg, decimal? declaredValue)
            : base(sender, recipient, weightKg, declaredValue)
        {
            Validate();
        }

        public override decimal GetWeightSurcharge()
        {
            return CalculateStandardWeightSurcharge();
        }

        public override decimal GetSpecialSurcharge()
        {
            return 50m;
        }

        public override void Validate()
        {
            if (WeightKg < 0.1m || WeightKg > 15m)
            {
                throw new InvalidOperationException("Fragile parcel weight must be from 0.1 kg to 15 kg.");
            }

            if (DeclaredValue == null || DeclaredValue <= 0)
            {
                throw new InvalidOperationException("Fragile parcel must have a declared value for mandatory insurance.");
            }
        }
    }
}
