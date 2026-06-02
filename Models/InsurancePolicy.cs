using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class InsurancePolicy
    {
        public decimal DeclaredValue { get; }
        public decimal Rate { get; } = 0.02m;

        public decimal Premium => CalculatePremium();

        public InsurancePolicy(decimal declaredValue)
        {
            if (declaredValue <= 0)
            {
                throw new InvalidOperationException("Declared value must be greater than zero.");
            }

            DeclaredValue = declaredValue;
        }

        public decimal CalculatePremium()
        {
            return DeclaredValue * Rate;
        }
    }
}
