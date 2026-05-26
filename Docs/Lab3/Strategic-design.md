# 📝 Laboratory Work №3: Domain Analysis and Transition from Layered Architecture to DDD
# Domain Analysis and Transition from Layered Architecture to DDD

- **Discipline:** Domain Engineering Technologies
- **Project:** BoardGameShop
- **Theme:** Transition from Classical Layered Architecture to Domain-Driven Design
- **Repository:** https://github.com/AvdikR/BoardGameShop

---

# 1. Project Overview

The `BoardGameShop` project represents an online board game store implemented as a RESTful Web API application using `.NET 10` and `ASP.NET Core`.

Initially, the system was designed using a classical layered architecture approach focused mainly on CRUD operations and database structure. During the analysis process, the project was reconsidered from a domain-oriented perspective in order to identify core business processes, business rules, and consistency boundaries.

The system currently supports:

- product catalog management;
- customer order processing;
- order lifecycle management;
- pricing and promotion calculation;
- reservation functionality for future gameplay sessions.

> Authentication and authorization are intentionally omitted at the current stage in order to focus primarily on domain modeling and business logic implementation.

---

# 2. Stage 1 — Domain Events, Commands, and Aggregates

## 2.1 Domain Events

Domain events describe meaningful business situations that already occurred inside the system.

### Catalog Context

- `ProductCreated`
- `ProductCatalogAssigned`
- `ProductStockChanged`
- `ProductPriceUpdated`
- `ProductMarkedForPreorder`

### Ordering Context

- `OrderCreated`
- `ProductAddedToOrder`
- `OrderConfirmed`
- `OrderPaid`
- `OrderCancelled`
- `OrderShipped`
- `OrderDelivered`
- `PromotionApplied`
- `LoyaltyDiscountApplied`
- `OrderTotalCalculated`

### Customer Context

- `CustomerInformationProvided`
- `CustomerLoyaltyTierUpdated`

### Reservation Context

- `ReservationCreated`
- `ReservationCancelled`
- `GameSessionReserved`
- `SpaceReserved`

---

## 2.2 Commands

Commands describe actions initiated by users or the system.

### Catalog Commands

- `CreateProduct`
- `UpdateProductPrice`
- `UpdateProductStock`
- `AssignProductToCatalog`
- `EnablePreorder`

### Ordering Commands

- `CreateOrder`
- `AddProductToOrder`
- `ConfirmOrder`
- `PayForOrder`
- `CancelOrder`
- `ShipOrder`
- `DeliverOrder`
- `ApplyPromotion`
- `CalculateOrderTotal`

### Customer Commands

- `ProvideCustomerInformation`
- `UpdateLoyaltyTier`

### Reservation Commands

- `CreateReservation`
- `ReserveGameSession`
- `ReserveSpace`
- `CancelReservation`

---

## 2.3 Aggregates

Aggregates define transactional and consistency boundaries inside the domain.

### Order Aggregate

The `Order` aggregate is the central business aggregate of the system.

It controls:

- order lifecycle transitions;
- validation of order state;
- consistency of order items;
- total price calculation;
- application of promotions and loyalty discounts.

The aggregate includes:

- `Order`
- `OrderItem`

`Order` acts as the Aggregate Root.

---

### Product Aggregate

The `Product` aggregate manages catalog information and product availability.

Responsibilities:

- stock quantity consistency;
- preorder availability;
- product pricing;
- catalog assignment.

---

### Customer Aggregate

The `Customer` aggregate stores customer contact information and loyalty-related data used during checkout operations.

---

### Reservation Aggregate

The `Reservation` aggregate manages booking operations for gameplay sessions and physical spaces.

Responsibilities:

- reservation validity;
- reservation status consistency;
- player capacity restrictions.

---

# 3. Stage 2 — Bounded Contexts

The system was divided into bounded contexts according to business responsibilities and semantic boundaries.

---

## 3.1 Catalog Context

The `Catalog Context` manages all information related to products visible to customers.

Inside this context, a `Product` represents a commercial item with pricing, stock visibility, preorder availability, and categorization.

### Responsibilities

- product management;
- catalog browsing;
- category organization;
- stock visibility.

### Main Terms

`Product`, `Catalog`, `Category`, `Price`, `Stock`, `Preorder`

---

## 3.2 Ordering Context

The `Ordering Context` is the core business context of the system.

Inside this context, an `Order` represents a complete purchase workflow rather than simply a database record.

The context manages:

- checkout operations;
- order state transitions;
- pricing rules;
- loyalty discounts;
- promotion calculation.

### Responsibilities

- order creation;
- order confirmation;
- price calculation;
- discount application;
- order lifecycle management.

### Main Terms

`Order`, `Checkout`, `Promotion`, `Discount`, `TotalPrice`, `Status`

---

## 3.3 Customer Context

The `Customer Context` manages customer-related information required during purchasing operations.

Currently, customer data is temporary and does not represent a fully authenticated account system.

### Responsibilities

- customer data management;
- loyalty tracking;
- linking customers with orders.

### Main Terms

`Customer`, `Contact Information`, `Email`, `Phone Number`, `LoyaltyTier`

---

## 3.4 Reservation Context

The `Reservation Context` supports future gameplay reservation functionality.

Inside this context, reservations represent booking operations rather than purchase transactions.

### Responsibilities

- reservation management;
- session booking;
- table reservation;
- reservation status tracking.

### Main Terms

`Reservation`, `GameSession`, `Space`, `ReservationStatus`, `Capacity`

---

# 4. Stage 3 — Ubiquitous Language

## 4.1 Catalog Context Language

| Term | Meaning |
|---|---|
| `Product` | Board game or related commercial item |
| `Catalog` | Collection of grouped products |
| `Category` | Product classification |
| `Stock` | Available amount of products |
| `Preorder` | Purchase before official release |
| `Price` | Commercial cost of a product |

---

## 4.2 Ordering Context Language

| Term | Meaning |
|---|---|
| `Order` | Customer purchase workflow |
| `Checkout` | Final confirmation process |
| `Promotion` | Discount rule applied to an order |
| `Loyalty Discount` | Discount based on customer tier |
| `TotalPrice` | Final calculated amount |
| `Status` | Current lifecycle state of an order |

---

## 4.3 Customer Context Language

| Term | Meaning |
|---|---|
| `Customer` | Person purchasing products |
| `LoyaltyTier` | Customer loyalty level |
| `Contact Information` | Communication data |
| `Email` | Customer email address |
| `Phone Number` | Customer contact number |

---

## 4.4 Reservation Context Language

| Term | Meaning |
|---|---|
| `Reservation` | Booking operation |
| `GameSession` | Organized gameplay activity |
| `Space` | Physical gameplay location |
| `Capacity` | Maximum allowed participants |
| `ReservationStatus` | Current reservation state |

---

# 5. Conclusion 🏁

During this laboratory work, the `BoardGameShop` project was analyzed from a domain-oriented perspective instead of a purely database-oriented structure.

The analysis demonstrated that the project contains several independent business subdomains and consistency boundaries that are not immediately visible in a classical layered architecture approach.

Through the identification of:

- domain events;
- commands;
- aggregates;
- bounded contexts;
- ubiquitous language;

the system was conceptually restructured toward `Domain-Driven Design` principles.

The resulting model provides a stronger foundation for future architectural evolution toward a more scalable and business-oriented system.
