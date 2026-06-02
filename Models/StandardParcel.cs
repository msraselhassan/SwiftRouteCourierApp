using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class StandardParcel : Parcel
    {
        public override bool CanBeInsured => true;
        public override bool RequiresInsurance => false;
        public override bool AllowsCod => true;

        public StandardParcel(Contact sender, Contact recipient, decimal weightKg, decimal? declaredValue = null)
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
            return 0m;
        }

        public override void Validate()
        {
            if (WeightKg < 0.1m || WeightKg > 30m)
            {
                throw new InvalidOperationException("Standard parcel weight must be from 0.1 kg to 30 kg.");
            }
        }
    }
}
