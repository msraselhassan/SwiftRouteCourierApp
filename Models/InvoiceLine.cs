using swiftroute_courier_app.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class InvoiceLine
    {
        public Shipment Shipment { get; }
        public decimal Amount { get; }
        public DateTime DeliveredAt { get; }

        public InvoiceLine(Shipment shipment)
        {
            if (shipment.Status != ShipmentStatus.Delivered)
            {
                throw new InvalidOperationException("Only delivered shipments can be added to invoice.");
            }

            Shipment = shipment;
            Amount = shipment.GetTotalPrice();
            DeliveredAt = shipment.DeliveredAt ?? DateTime.Now;
        }

        public void Print()
        {
            Console.WriteLine($"{Shipment.ShipmentId} | {Shipment.Parcel.GetType().Name} | {DeliveredAt:d} | {Amount} BDT");
        }
    }
}
