using swiftroute_courier_app.Enums;
using swiftroute_courier_app.Models;



DeliveryHub hub = new DeliveryHub();




Console.WriteLine("1. REGISTER CUSTOMERS");
Console.WriteLine("---------------------");

IndividualCustomer individualCustomer = new IndividualCustomer(
    "Rahim",
    "01711111111"
);

BusinessCustomer businessCustomer = new BusinessCustomer(
    "Karim",
    "01822222222",
    "ABC Electronics",
    new CreditAccount("BA-1001", 100000m)
);

hub.RegisterCustomer(individualCustomer);
hub.RegisterCustomer(businessCustomer);

Console.WriteLine();



Console.WriteLine("REGISTER COURIERS");
Console.WriteLine("-----------------");

BikeRider bikeRider = new BikeRider("Bike Rider-Hasan", "01911111111");
VanDriver vanDriver = new VanDriver("Van Driver-Selim", "01922222222");
TruckDriver truckDriver = new TruckDriver("Truck Driver-Babul", "01933333333");

hub.RegisterCourier(bikeRider);
hub.RegisterCourier(vanDriver);
hub.RegisterCourier(truckDriver);

Console.WriteLine();



Address senderAddress = new Address(
    "House 10",
    "Road 5",
    "Dhanmondi",
    "Dhaka",
    "Dhaka"
);

Address recipientAddress = new Address(
    "House 22",
    "Road 8",
    "Agrabad",
    "Chattogram",
    "Chattogram"
);

Contact sender = new Contact(
    "Uddin",
    "01711111111",
    senderAddress
);

Contact recipient = new Contact(
    "Sabbir",
    "01633333333",
    recipientAddress
);



Console.WriteLine("2. BOOK SHIPMENTS");
Console.WriteLine("-----------------");


Shipment documentShipment = hub.BookShipment(
    individualCustomer,
    new DocumentParcel(sender, recipient, 0.3m),
    ServiceTier.NextDay
);


Shipment standardInsuredShipment = hub.BookShipment(
    individualCustomer,
    new StandardParcel(sender, recipient, 4m, 10000m),
    ServiceTier.NextDay,
    new InsurancePolicy(10000m)
);


Shipment fragileShipment = hub.BookShipment(
    businessCustomer,
    new FragileParcel(sender, recipient, 10m, 20000m),
    ServiceTier.Economy,
    new InsurancePolicy(20000m)
);


Shipment codShipment = hub.BookShipment(
    individualCustomer,
    new StandardParcel(sender, recipient, 2m, 5000m),
    ServiceTier.SameDay,
    null,
    new CashOnDelivery(3000m)
);


Shipment heavyShipment = hub.BookShipment(
    individualCustomer,
    new StandardParcel(sender, recipient, 20m, 15000m),
    ServiceTier.NextDay
);


Shipment refridgeratedShipment = hub.BookShipment(
    individualCustomer,
    new RefrigeratedParcel(sender, recipient,12m,8000m), 
    ServiceTier.NextDay
);


Shipment vipDocumentShipment = hub.BookShipment(
    individualCustomer,
    new DocumentParcel(sender, recipient, 0.3m, 30000m),
    ServiceTier.Vip,
    new InsurancePolicy(30000m)
);

Console.WriteLine();



Console.WriteLine("3. PRICE BREAKDOWN FOR EACH SHIPMENT");
Console.WriteLine("------------------------------------");

Console.WriteLine("Document Shipment:");
documentShipment.PrintPriceBreakdown();

Console.WriteLine("Standard Insured Shipment:");
standardInsuredShipment.PrintPriceBreakdown();

Console.WriteLine("Fragile Shipment:");
fragileShipment.PrintPriceBreakdown();

Console.WriteLine("COD Shipment:");
codShipment.PrintPriceBreakdown();

Console.WriteLine("Refridgment Shipment:");
refridgeratedShipment.PrintPriceBreakdown();

Console.WriteLine("Vip Shipment:");
vipDocumentShipment.PrintPriceBreakdown();



Console.WriteLine();



Console.WriteLine("4. ATTEMPT INVALID BIKE ASSIGNMENT");
Console.WriteLine("----------------------------------");

try
{
    hub.AssignCourier(heavyShipment, bikeRider);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Assignment refused successfully.");
    Console.WriteLine($"Reason: {ex.Message}");
}

try
{
    hub.AssignCourier(refridgeratedShipment, bikeRider);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Bike refused refrigerated parcel successfully.");
    Console.WriteLine($"Reason: {ex.Message}");
}


try
{
    hub.AssignCourier(vipDocumentShipment, bikeRider);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Bike refused VIP shipment successfully.");
    Console.WriteLine($"Reason: {ex.Message}");
}

Console.WriteLine();




Console.WriteLine("ASSIGN VALID COURIERS");
Console.WriteLine("---------------------");

hub.AssignCourier(documentShipment, bikeRider);
hub.AssignCourier(standardInsuredShipment, vanDriver);
hub.AssignCourier(fragileShipment, vanDriver);
hub.AssignCourier(codShipment, bikeRider);
hub.AssignCourier(refridgeratedShipment,vanDriver);

hub.AssignCourier(vipDocumentShipment, vanDriver);

Console.WriteLine();




Console.WriteLine("5. WALK DOCUMENT SHIPMENT TO DELIVERED");
Console.WriteLine("--------------------------------------");

Console.WriteLine($"Current Status: {documentShipment.Status}");

documentShipment.MarkPickedUp();
Console.WriteLine($"After Pickup: {documentShipment.Status}");

documentShipment.MarkInTransit();
Console.WriteLine($"After InTransit: {documentShipment.Status}");

documentShipment.MarkOutForDelivery();
Console.WriteLine($"After OutForDelivery: {documentShipment.Status}");

documentShipment.MarkDelivered();
Console.WriteLine($"After Delivered: {documentShipment.Status}");

Console.WriteLine();



Console.WriteLine("MAKE BUSINESS SHIPMENT DELIVERED FOR INVOICE");
Console.WriteLine("--------------------------------------------");

fragileShipment.MarkPickedUp();
Console.WriteLine($"Fragile Status: {fragileShipment.Status}");

fragileShipment.MarkInTransit();
Console.WriteLine($"Fragile Status: {fragileShipment.Status}");

fragileShipment.MarkOutForDelivery();
Console.WriteLine($"Fragile Status: {fragileShipment.Status}");

fragileShipment.MarkDelivered();
Console.WriteLine($"Fragile Status: {fragileShipment.Status}");

Console.WriteLine();




Console.WriteLine("6. WALK STANDARD SHIPMENT TO FAILED THEN RETURNED");
Console.WriteLine("------------------------------------------------");

Console.WriteLine($"Current Status: {standardInsuredShipment.Status}");

standardInsuredShipment.MarkPickedUp();
Console.WriteLine($"After Pickup: {standardInsuredShipment.Status}");

standardInsuredShipment.MarkInTransit();
Console.WriteLine($"After InTransit: {standardInsuredShipment.Status}");

standardInsuredShipment.MarkOutForDelivery();
Console.WriteLine($"After OutForDelivery: {standardInsuredShipment.Status}");

standardInsuredShipment.MarkFailed();
Console.WriteLine($"After Failed: {standardInsuredShipment.Status}");

standardInsuredShipment.MarkReturned();
Console.WriteLine($"After Returned: {standardInsuredShipment.Status}");

Console.WriteLine();




Console.WriteLine("7. ATTEMPT ILLEGAL STATUS TRANSITION");
Console.WriteLine("------------------------------------");

try
{
    documentShipment.MarkInTransit();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Illegal transition refused successfully.");
    Console.WriteLine($"Reason: {ex.Message}");
}

Console.WriteLine();




hub.PrintShipmentsInTransit();
hub.PrintDeliveredToday();

Console.WriteLine();




Console.WriteLine("8. MONTH-END INVOICE FOR BUSINESS CUSTOMER");
Console.WriteLine("------------------------------------------");

Invoice invoice = hub.GenerateMonthlyInvoice(
    businessCustomer,
    DateTime.Today.Month,
    DateTime.Today.Year
);

invoice.Print();

Console.WriteLine();




