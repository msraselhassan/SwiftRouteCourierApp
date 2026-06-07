using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public abstract class Parcel
    {
        public Contact Sender { get; }
        public Contact Recipient { get; }
        public decimal WeightKg { get; }
        public decimal? DeclaredValue { get; }

        public abstract bool CanBeInsured { get; }
        public abstract bool RequiresInsurance { get; }
        public abstract bool AllowsCod { get; }

        protected Parcel(Contact sender, Contact recipient, decimal weightKg, decimal? declaredValue)
        {
            Sender = sender;
            Recipient = recipient;
            WeightKg = weightKg;
            DeclaredValue = declaredValue;
        }

        public abstract decimal GetWeightSurcharge();
        public abstract decimal GetSpecialSurcharge();
        public abstract void Validate();

        protected decimal CalculateStandardWeightSurcharge()
        {
            if (WeightKg <= 1m)
            {
                return 0m;
            }

            if (WeightKg <= 5m)
            {
                return 30m;
            }

            if (WeightKg <= 15m)
            {
                return 80m;
            }


            return 150m;
        }

        public override string ToString()
        {
            return $"{GetType().Name}, Weight: {WeightKg} kg";
        }
    }
}
