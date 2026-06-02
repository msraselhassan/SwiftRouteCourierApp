using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Enums
{
    public enum ServiceTier
    {
        SameDay,
        NextDay,
        Economy
    }

    public enum ShipmentStatus
    {
        Booked,
        PickedUp,
        InTransit,
        OutForDelivery,
        Delivered,
        Failed,
        Returned,
        Cancelled
    }

    public enum PaymentMode
    {
        CashAtPickup,
        CashOnDelivery,
        MonthlyCredit
    }
}
