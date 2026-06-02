using swiftroute_courier_app.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class DeliveryHub
    {
        private readonly List<Customer> _customers = new();
        private readonly List<Courier> _couriers = new();
        private readonly List<Shipment> _shipments = new();

        public IReadOnlyList<Customer> Customers => _customers;
        public IReadOnlyList<Courier> Couriers => _couriers;
        public IReadOnlyList<Shipment> Shipments => _shipments;

        public void RegisterCustomer(Customer customer)
        {
            _customers.Add(customer);
            Console.WriteLine($"Customer registered: {customer}");
        }

        public void RegisterCourier(Courier courier)
        {
            _couriers.Add(courier);
            Console.WriteLine($"Courier registered: {courier}");
        }

        public Shipment BookShipment(
            Customer customer,
            Parcel parcel,
            ServiceTier serviceTier,
            InsurancePolicy? insurancePolicy = null,
            CashOnDelivery? cashOnDelivery = null)
        {
            Shipment shipment = new Shipment(
                customer,
                parcel,
                serviceTier,
                insurancePolicy,
                cashOnDelivery);

            _shipments.Add(shipment);

            Console.WriteLine($"Shipment booked: {shipment.ShipmentId}");

            return shipment;
        }

        public void AssignCourier(Shipment shipment, Courier courier)
        {
            shipment.AssignCourier(courier);
            Console.WriteLine($"Courier {courier.Name} assigned to shipment {shipment.ShipmentId}");
        }

        public void PrintShipmentsInTransit()
        {
            Console.WriteLine();
            Console.WriteLine("========== SHIPMENTS CURRENTLY IN TRANSIT ==========");

            var inTransitShipments = _shipments
                .Where(s =>
                    s.Status == ShipmentStatus.PickedUp ||
                    s.Status == ShipmentStatus.InTransit ||
                    s.Status == ShipmentStatus.OutForDelivery)
                .ToList();

            foreach (Shipment shipment in inTransitShipments)
            {
                Console.WriteLine(shipment);
            }

            Console.WriteLine("====================================================");
        }

        public void PrintDeliveredToday()
        {
            Console.WriteLine();
            Console.WriteLine("========== DELIVERED TODAY ==========");

            DateTime today = DateTime.Today;

            var deliveredToday = _shipments
                .Where(s =>
                    s.Status == ShipmentStatus.Delivered &&
                    s.DeliveredAt != null &&
                    s.DeliveredAt.Value.Date == today)
                .ToList();

            foreach (Shipment shipment in deliveredToday)
            {
                Console.WriteLine(shipment);
            }

            decimal totalRevenue = deliveredToday.Sum(s => s.GetTotalPrice());

            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"Total Revenue: {totalRevenue} BDT");
            Console.WriteLine("=====================================");
        }

        public Invoice GenerateMonthlyInvoice(BusinessCustomer customer, int month, int year)
        {
            Invoice invoice = new Invoice(customer, month, year);

            var deliveredShipments = _shipments
                .Where(s =>
                    s.Customer == customer &&
                    s.Status == ShipmentStatus.Delivered &&
                    s.DeliveredAt != null &&
                    s.DeliveredAt.Value.Month == month &&
                    s.DeliveredAt.Value.Year == year)
                .ToList();

            foreach (Shipment shipment in deliveredShipments)
            {
                InvoiceLine line = new InvoiceLine(shipment);
                invoice.AddLine(line);
            }

            customer.CreditAccount.AddCharge(invoice.TotalAmount);

            return invoice;
        }
    }
}
