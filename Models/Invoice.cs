using System;
using System.Collections.Generic;
using System.Text;

namespace swiftroute_courier_app.Models
{
    public class Invoice
    {
        public Guid InvoiceId { get; } = Guid.NewGuid();
        public BusinessCustomer Customer { get; }
        public int Month { get; }
        public int Year { get; }

        public List<InvoiceLine> Lines { get; } = new();

        public decimal TotalAmount => Lines.Sum(line => line.Amount);

        public Invoice(BusinessCustomer customer, int month, int year)
        {
            Customer = customer;
            Month = month;
            Year = year;
        }

        public void AddLine(InvoiceLine line)
        {
            Lines.Add(line);
        }

        public void Print()
        {
            Console.WriteLine();
            Console.WriteLine("========== MONTH-END BUSINESS INVOICE ==========");
            Console.WriteLine($"Invoice ID : {InvoiceId}");
            Console.WriteLine($"Customer   : {Customer.CompanyName}");
            Console.WriteLine($"Month/Year : {Month}/{Year}");
            Console.WriteLine("-----------------------------------------------");

            foreach (InvoiceLine line in Lines)
            {
                line.Print();
            }

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine($"Consolidated Total: {TotalAmount} BDT");
            Console.WriteLine("================================================");
        }
    }
}
