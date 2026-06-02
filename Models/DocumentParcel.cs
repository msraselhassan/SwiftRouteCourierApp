using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class DocumentParcel : Parcel
    {
        public override bool CanBeInsured => false;
        public override bool RequiresInsurance => false;
        public override bool AllowsCod => false;

        public DocumentParcel(Contact sender, Contact recipient, decimal weightKg, decimal? declaredValue = null)
            : base(sender, recipient, weightKg, declaredValue)
        {
            Validate();
        }

        public override decimal GetWeightSurcharge()
        {
            return 0m;
        }

        public override decimal GetSpecialSurcharge()
        {
            return 0m;
        }

        public override void Validate()
        {
            if (WeightKg <= 0 || WeightKg >= 0.5m)
            {
                throw new InvalidOperationException("Document parcel must be under 0.5 kg.");
            }
        }
    }
}
