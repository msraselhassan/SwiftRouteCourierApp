

## 1. Class list

The final implementation uses a simplified domain model. I removed separate report classes, service classes, and custom exception classes to keep the project easier to understand and defend.

### `DeliveryHub`

`DeliveryHub` is the main coordinator of the system. It stores customers, couriers, and shipments in memory, books shipments, assigns couriers, prints simple reports, and generates the business invoice.

### `Shipment`

`Shipment` represents one delivery order. It connects the customer, parcel, service tier, optional insurance, optional COD, assigned courier, current status, pricing calculation, and lifecycle behaviour.

### `Parcel`

`Parcel` is an abstract base class for all parcel types. It stores common parcel information such as sender, recipient, weight, and declared value, while forcing child classes to define parcel-specific rules.

### `DocumentParcel`

`DocumentParcel` represents document deliveries such as passports or certificates. It enforces that documents must be under 0.5 kg, cannot be insured, cannot use COD, and have no weight surcharge.

### `StandardParcel`

`StandardParcel` represents normal parcels such as clothes, books, electronics, and gifts. It supports optional insurance, allows COD, and uses the normal weight surcharge rules up to 30 kg.

### `FragileParcel`

`FragileParcel` represents fragile items such as glassware or lab equipment. It requires insurance, cannot exceed 15 kg, uses the normal weight surcharge rules, and adds a flat fragile surcharge.

### `Contact`

`Contact` stores the name, phone number, and address for either a sender or a recipient.

### `Address`

`Address` stores the location details used for pickup and delivery.

### `Customer`

`Customer` is an abstract base class for all customer types. It stores common customer information and defines common behaviours such as discount rate, COD eligibility, and payment mode.

### `IndividualCustomer`

`IndividualCustomer` represents a normal customer who pays per parcel and can use Cash on Delivery.

### `BusinessCustomer`

`BusinessCustomer` represents a company customer. It has a company name, a credit account, receives a 10% discount, cannot use COD, and is billed using a month-end invoice.

### `CreditAccount`

`CreditAccount` stores the business customer’s credit account number, balance, and credit limit.

### `Courier`

`Courier` is an abstract base class for all courier types. It stores common courier information and requires each child courier type to implement its own `CanAccept` rule.

### `BikeRider`

`BikeRider` represents a bike courier. It can only accept shipments up to 5 kg and only for Same-Day or Next-Day service tiers.

### `VanDriver`

`VanDriver` represents a van courier. It can accept parcels up to 50 kg and can work with any service tier.

### `TruckDriver`

`TruckDriver` represents a truck courier. It is intended for Economy shipments and only dispatches when the cargo is at least 20 kg.

### `InsurancePolicy`

`InsurancePolicy` stores the declared value and calculates the insurance premium as 2% of the declared value.

### `CashOnDelivery`

`CashOnDelivery` stores the COD amount and calculates the 1% COD service fee and the remittance amount to the sender.

### `Invoice`

`Invoice` represents a month-end invoice for a business customer. It contains invoice lines and calculates the consolidated total.

### `InvoiceLine`

`InvoiceLine` represents one delivered shipment inside a business invoice.

### `ServiceTier`

`ServiceTier` is an enum that represents Same-Day, Next-Day, and Economy delivery choices.

### `ShipmentStatus`

`ShipmentStatus` is an enum that represents the allowed shipment states such as Booked, PickedUp, InTransit, OutForDelivery, Delivered, Failed, Returned, and Cancelled.

### `PaymentMode`

`PaymentMode` is an enum that represents CashAtPickup, CashOnDelivery, and MonthlyCredit.

---



## 2. Inheritance vs composition

### Inheritance used

I used inheritance where there is a clear **is-a** relationship and where the child classes share some common data but have different business rules.

#### Parcel inheritance

```text
Parcel
├── DocumentParcel
├── StandardParcel
└── FragileParcel


This is inheritance because a document parcel, standard parcel, and fragile parcel are all parcels.
They all share sender, recipient, weight, and declared value, but each type has different rules for insurance, COD, weight validation, 
and surcharges. Making Parcel abstract prevents the system from creating a generic parcel without a real parcel category.

Customer inheritance
Customer
├── IndividualCustomer
└── BusinessCustomer

This is inheritance because both individual and business customers are customers.
They share name and phone information, but they behave differently. Individual customers can use COD and do not receive a discount.
Business customers cannot use COD, receive a 10% discount, and use a credit account.

Courier inheritance 
Courier
├── BikeRider
├── VanDriver
└── TruckDriver

This is inheritance because bike riders, van drivers, and truck drivers are all couriers. 
They share courier identity, name, and phone number, but each one has different rules for accepting shipments. 
The abstract method CanAccept allows each courier type to enforce its own rule.

Composition and association used

I used composition or association where one object has another object instead of being another object.

DeliveryHub has many customers, couriers, and shipments

DeliveryHub manages collections of customers, couriers, and shipments. One hub can manage many customers, many couriers, and many shipments.

Shipment has one customer

A shipment belongs to one customer. This is not inheritance because a shipment is not a customer; it only refers to the customer who booked it.

Shipment has one parcel

A shipment contains exactly one parcel. This is composition because the parcel is the item being delivered as part of that shipment.

Shipment may have one courier

A shipment starts without a courier and later may be assigned one courier. One courier can handle many shipments over time, but each shipment has at most one assigned courier in this simplified design.

Shipment may have one insurance policy

Insurance is optional for standard parcels, mandatory for fragile parcels, and unavailable for document parcels. Therefore, the relationship is optional.

Shipment may have one COD object

COD is optional and only allowed for individual customers and non-document parcels. It is a separate class because it has its own amount, fee, and remittance calculation.

Parcel has sender and recipient contacts

A parcel has a sender contact and a recipient contact. These are not parcel types, so composition is more appropriate than inheritance.

Contact has one address

A contact owns address information, so Address is placed inside Contact.

BusinessCustomer has one credit account

A business customer owns one credit account because business customers use monthly credit billing.

Invoice has many invoice lines

An invoice is made from invoice lines. Each invoice line points to one delivered shipment.


3. Where lifecycle rules live

The lifecycle rules live inside the Shipment class.

The rule that Delivered is terminal is owned by Shipment, mainly inside the CanMoveTo and MoveTo methods. In my implementation, if the current status is Delivered, Returned, or Cancelled, then the shipment cannot move to any other status.

This belongs inside Shipment because status is part of a shipment’s own state. A shipment should protect its own lifecycle instead of allowing outside code to freely change its status. For example, the program should not be able to directly set a delivered shipment back to InTransit.

The public methods such as:

MarkPickedUp()
MarkInTransit()
MarkOutForDelivery()
MarkDelivered()
MarkFailed()
MarkReturned()
Cancel()

make the allowed business actions clear. Internally, they call the private MoveTo method, which checks whether the transition is legal. If a transition is illegal, the code throws a built-in InvalidOperationException.

This design keeps lifecycle validation close to the data it protects.

4. Where pricing rules live

In the simplified implementation, pricing rules live mainly inside the Shipment class, with some parcel-specific surcharge rules inside the parcel classes.

The base rate by service tier, insurance charge, subtotal, business discount, and total price are calculated in Shipment.

Pricing inside parcel classes

Parcel-specific pricing parts remain inside the parcel classes. For example:

DocumentParcel returns no weight surcharge and no special surcharge.
StandardParcel uses the normal weight surcharge rules.
FragileParcel uses the normal weight surcharge rules and adds the fragile surcharge.

This means Shipment controls the full bill, but each parcel type controls the part of pricing that belongs to that parcel type.


5. One alternative considered and rejected

One alternative I considered was to create one general Parcel class with a ParcelType enum instead of using inheritance.

class Parcel
{
    public ParcelType Type { get; set; }
}

Then the code would check the parcel type using many if or switch statements:

if (parcel.Type == ParcelType.Document)
{
    // document rules
}
else if (parcel.Type == ParcelType.Fragile)
{
    // fragile rules
}

I rejected this approach because it puts too many unrelated rules into one class. The document rules, standard parcel rules, and fragile parcel rules would all be mixed together. If a new parcel type like RefrigeratedParcel is added later, I would need to update many switch statements in different places.

Instead, I used an abstract Parcel class with separate child classes. This keeps each parcel type responsible for its own validation and surcharge behaviour. It is easier to read, easier to test, and easier to explain.


Overall, this design uses inheritance for real is-a relationships, composition for has-a relationships, and keeps the main business rules inside the objects that own the data.







## classes Relationship:

1. DeliveryHub relationships

DeliveryHub is the central manager.

DeliveryHub 1 ---- 0..* Customer
DeliveryHub 1 ---- 0..* Courier
DeliveryHub 1 ---- 0..* Shipment
DeliveryHub 1 ---- 0..* Invoice

2. Customer relationships

 Customer
├── IndividualCustomer
└── BusinessCustomer

3. IndividualCustomer relationships

IndividualCustomer ----|> Customer

4. BusinessCustomer relationships

BusinessCustomer ----|> Customer
BusinessCustomer 1 ---- 1 CreditAccount
BusinessCustomer 1 ---- 0..* Invoice

5. CreditAccount relationships

BusinessCustomer 1 ---- 1 CreditAccount

6. Parcel relationships

Parcel
├── DocumentParcel
├── StandardParcel
└── FragileParcel

Parcel 1 ---- 1 Sender Contact
Parcel 1 ---- 1 Recipient Contact

7. DocumentParcel relationships

DocumentParcel ----|> Parcel

8. StandardParcel relationships
StandardParcel ----|> Parcel

9. FragileParcel relationships
 FragileParcel ----|> Parcel

10. Contact relationships

Parcel 1 ---- 1 Sender Contact
Parcel 1 ---- 1 Recipient Contact
Contact 1 ---- 1 Address

11. Address relationships
 Contact 1 ---- 1 Address

12. Courier relationships

Courier
├── BikeRider
├── VanDriver
└── TruckDriver

Courier 1 ---- 0..* Shipment
Shipment 0..1 ---- 1 Courier

13. BikeRider relationships
	BikeRider ----|> Courier

14. VanDriver relationships
	VanDriver ----|> Courier

15. TruckDriver relationships
	TruckDriver ----|> Courier

16. Shipment relationships
	
Customer 1 ---- 0..* Shipment
Shipment 1 ---- 1 Parcel
Shipment 0..* ---- 0..1 Courier
Shipment 1 ---- 0..1 InsurancePolicy
Shipment 1 ---- 0..1 CashOnDelivery
Shipment 1 ---- 1 ServiceTier
Shipment 1 ---- 1 ShipmentStatus
InvoiceLine 0..* ---- 1 Shipment

17. InsurancePolicy relationships
	Shipment 1 ---- 0..1 InsurancePolicy

18. CashOnDelivery relationships
	Shipment 1 ---- 0..1 CashOnDelivery

19. Invoice relationships

BusinessCustomer 1 ---- 0..* Invoice
Invoice 1 ---- 0..* InvoiceLine
Invoice 0..* ---- 1 BusinessCustomer

20. InvoiceLine relationships

Invoice 1 ---- 0..* InvoiceLine
InvoiceLine 1 ---- 1 Shipment


21.Additional Enum class