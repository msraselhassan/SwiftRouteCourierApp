

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