using swiftroute_courier_app.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class Shipment
    {
        public Guid ShipmentId { get; } = Guid.NewGuid();

        public Customer Customer { get; }
        public Parcel Parcel { get; }
        public ServiceTier ServiceTier { get; }

        public ShipmentStatus Status { get; private set; } = ShipmentStatus.Booked;

        public Courier? AssignedCourier { get; private set; }
        public InsurancePolicy? InsurancePolicy { get; }
        public CashOnDelivery? CashOnDelivery { get; }

        public DateTime BookedAt { get; } = DateTime.Now;
        public DateTime? PickedUpAt { get; private set; }
        public DateTime? DeliveredAt { get; private set; }
        public DateTime? ReturnedAt { get; private set; }

        public Shipment(
            Customer customer,
            Parcel parcel,
            ServiceTier serviceTier,
            InsurancePolicy? insurancePolicy = null,
            CashOnDelivery? cashOnDelivery = null)
        {
            Customer = customer;
            Parcel = parcel;
            ServiceTier = serviceTier;
            InsurancePolicy = insurancePolicy;
            CashOnDelivery = cashOnDelivery;

            ValidateBookingRules();
        }

        private void ValidateBookingRules()
        {
            if (InsurancePolicy != null && !Parcel.CanBeInsured)
            {
                throw new InvalidOperationException("This parcel type cannot be insured.");
            }

            if (Parcel.RequiresInsurance && InsurancePolicy == null)
            {
                throw new InvalidOperationException("This parcel type requires insurance.");
            }

            if (CashOnDelivery != null && !Customer.CanUseCod)
            {
                throw new InvalidOperationException("Business customers cannot use COD.");
            }

            if (CashOnDelivery != null && !Parcel.AllowsCod)
            {
                throw new InvalidOperationException("This parcel type cannot use COD.");
            }

            if (InsurancePolicy != null && Parcel.DeclaredValue == null)
            {
                throw new InvalidOperationException("Declared value is required for insurance.");
            }
        }

        public void AssignCourier(Courier courier)
        {
            if (!courier.CanAccept(this))
            {
                throw new InvalidOperationException($"{courier.GetType().Name} cannot accept this shipment.");
            }

            AssignedCourier = courier;
        }

        public void MarkPickedUp()
        {
            MoveTo(ShipmentStatus.PickedUp);
            PickedUpAt = DateTime.Now;
        }

        public void MarkInTransit()
        {
            MoveTo(ShipmentStatus.InTransit);
        }

        public void MarkOutForDelivery()
        {
            MoveTo(ShipmentStatus.OutForDelivery);
        }

        public void MarkDelivered()
        {
            MoveTo(ShipmentStatus.Delivered);
            DeliveredAt = DateTime.Now;
        }

        public void MarkFailed()
        {
            MoveTo(ShipmentStatus.Failed);
        }

        public void MarkReturned()
        {
            MoveTo(ShipmentStatus.Returned);
            ReturnedAt = DateTime.Now;
        }

        public void Cancel()
        {
            MoveTo(ShipmentStatus.Cancelled);
        }

        private void MoveTo(ShipmentStatus nextStatus)
        {
            if (!CanMoveTo(nextStatus))
            {
                throw new InvalidOperationException($"Invalid status change: {Status} to {nextStatus}");
            }

            Status = nextStatus;
        }

        public bool CanMoveTo(ShipmentStatus nextStatus)
        {
            if (Status == ShipmentStatus.Delivered ||
                Status == ShipmentStatus.Returned ||
                Status == ShipmentStatus.Cancelled)
            {
                return false;
            }

            return Status switch
            {
                ShipmentStatus.Booked =>
                    nextStatus == ShipmentStatus.PickedUp ||
                    nextStatus == ShipmentStatus.Cancelled,

                ShipmentStatus.PickedUp =>
                    nextStatus == ShipmentStatus.InTransit,

                ShipmentStatus.InTransit =>
                    nextStatus == ShipmentStatus.OutForDelivery,

                ShipmentStatus.OutForDelivery =>
                    nextStatus == ShipmentStatus.Delivered ||
                    nextStatus == ShipmentStatus.Failed,

                ShipmentStatus.Failed =>
                    nextStatus == ShipmentStatus.Returned,

                _ => false
            };
        }

        public decimal GetBaseRate()
        {
            return ServiceTier switch
            {
                ServiceTier.SameDay => 200m,
                ServiceTier.NextDay => 100m,
                ServiceTier.Economy => 60m,
                _ => 0m
            };
        }

        public decimal GetWeightSurcharge()
        {
            return Parcel.GetWeightSurcharge();
        }

        public decimal GetSpecialSurcharge()
        {
            return Parcel.GetSpecialSurcharge();
        }

        public decimal GetInsuranceCharge()
        {
            return InsurancePolicy?.CalculatePremium() ?? 0m;
        }

        public decimal GetSubtotal()
        {
            return GetBaseRate()
                   + GetWeightSurcharge()
                   + GetSpecialSurcharge()
                   + GetInsuranceCharge();
        }

        public decimal GetBusinessDiscount()
        {
            return GetSubtotal() * Customer.DiscountRate;
        }

        public decimal GetTotalPrice()
        {
            return GetSubtotal() - GetBusinessDiscount();
        }

        public void PrintPriceBreakdown()
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine($"Shipment ID       : {ShipmentId}");
            Console.WriteLine($"Customer          : {Customer}");
            Console.WriteLine($"Parcel Type       : {Parcel.GetType().Name}");
            Console.WriteLine($"Service Tier      : {ServiceTier}");
            Console.WriteLine($"Weight            : {Parcel.WeightKg} kg");
            Console.WriteLine($"Base Rate         : {GetBaseRate()} BDT");
            Console.WriteLine($"Weight Surcharge  : {GetWeightSurcharge()} BDT");
            Console.WriteLine($"Special Surcharge : {GetSpecialSurcharge()} BDT");
            Console.WriteLine($"Insurance Charge  : {GetInsuranceCharge()} BDT");
            Console.WriteLine($"Subtotal          : {GetSubtotal()} BDT");
            Console.WriteLine($"Business Discount : {GetBusinessDiscount()} BDT");
            Console.WriteLine($"Total Price       : {GetTotalPrice()} BDT");

            if (CashOnDelivery != null)
            {
                Console.WriteLine($"COD Amount        : {CashOnDelivery.Amount} BDT");
                Console.WriteLine($"COD Fee           : {CashOnDelivery.ServiceFee} BDT");
                Console.WriteLine($"Sender Remittance : {CashOnDelivery.RemittanceAmount} BDT");
            }

            Console.WriteLine("--------------------------------------");
        }

        public override string ToString()
        {
            return $"{ShipmentId} | {Parcel.GetType().Name} | {ServiceTier} | {Status} | {GetTotalPrice()} BDT";
        }
    }
}
