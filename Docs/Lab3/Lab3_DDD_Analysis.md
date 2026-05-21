# 📝 Laboratory Work №3: Domain Analysis and Transition from Layered Architecture to DDD

- **Discipline:** Domain Engineering Technologies
- **Project:** BoardGameShop
- **Theme:** Transition from Classical Layered Architecture to Domain-Driven Design
- **Repository:** [BoardGameShop on GitHub](https://github.com/AvdikR/BoardGameShop)

---

## 1. Project Overview

The project represents an online board game store implemented as a RESTful Web API application using .NET 10. The system currently focuses on the core e-commerce workflow: browsing products, selecting games, creating orders, and confirming purchases.

> At the current stage, the project does not implement authentication or authorization. Customer information is entered dynamically during order creation. This allows the team to focus primarily on business logic, domain modeling, and architectural organization.

The domain also includes planned extensions related to board game reservations, DnD sessions, and renting physical spaces.

---

## 2. Stage 1 — Domain Events, Commands, and Aggregates

### 2.1 Domain Events

Domain events describe important business situations that already happened in the system.

- **Catalog Context:** `ProductCreated`, `ProductUpdated`, `ProductRemoved`, `ProductAddedToCatalog`, `ProductStockUpdated`
- **Ordering Context:** `OrderCreated`, `ProductAddedToOrder`, `ProductRemovedFromOrder`, `OrderConfirmed`, `OrderCancelled`, `OrderTotalCalculated`
- **Customer Context:** `CustomerInformationProvided`, `CustomerDataUpdated`
- **Reservation Context:** `ReservationCreated`, `ReservationCancelled`, `ReservationCompleted`, `GameSessionReserved`, `SpaceReserved`

### 2.2 Commands

Commands represent actions initiated by a user or the system that lead to domain events.

- **Catalog:** `CreateProduct`, `UpdateProduct`, `DeleteProduct`, `AddProductToCatalog`, `UpdateProductStock`
- **Ordering:** `CreateOrder`, `AddProductToOrder`, `RemoveProductFromOrder`, `ConfirmOrder`, `CancelOrder`, `CalculateOrderTotal`
- **Customer:** `ProvideCustomerInformation`, `UpdateCustomerInformation`
- **Reservation:** `CreateReservation`, `CancelReservation`, `ReserveGameSession`, `ReserveSpace`

### 2.3 Aggregates

Aggregates group related business operations and protect consistency boundaries.

- **Product Aggregate:** Responsible for product information, pricing, stock quantity, preorder availability, and catalog assignment.
- **Order Aggregate:** Controls the order lifecycle and ensures consistency of order items and total price calculation (includes `Order` and `OrderItem` entities).
- **Customer Aggregate:** Manages customer contact information used during order confirmation and reservation creation.
- **Reservation Aggregate:** Responsible for booking operations related to game sessions and physical spaces.

---

## 3. Stage 2 — Bounded Contexts

### 3.1 Catalog Context

Responsible for storing and managing product information visible to customers.

- **Responsibilities:** Product management, categorization, catalog browsing, stock visibility.
- **Main Terms:** `Product`, `Catalog`, `Category`, `Price`, `Stock`.

### 3.2 Ordering Context

Handles customer purchases and order processing.

- **Responsibilities:** Order creation, adding products, calculating totals, order confirmation, status management.
- **Main Terms:** `Order`, `OrderItem`, `Checkout`, `TotalPrice`, `Status`.

### 3.3 Customer Context

Responsible for storing temporary customer contact information used during order confirmation.

- **Responsibilities:** Storing and updating customer data, linking customers with orders.
- **Main Terms:** `Customer`, `Contact Information`, `Email`, `Phone Number`.

### 3.4 Reservation Context

Supports future functionality related to reserving spaces and game sessions.

- **Responsibilities:** Reservation management, booking game sessions, reserving physical spaces, tracking status.
- **Main Terms:** `Reservation`, `GameSession`, `Space`, `ReservationStatus`, `SessionDate`.

---

## 4. Stage 3 — Ubiquitous Language

### 4.1 Catalog Context Language

| Term         | Meaning                                             |
| :----------- | :-------------------------------------------------- |
| **Product**  | A board game or related item available in the store |
| **Catalog**  | Collection of products grouped by categories        |
| **Category** | Product grouping type                               |
| **Price**    | Cost of a product                                   |
| **Stock**    | Available amount of products                        |
| **Preorder** | Ability to purchase a product before release        |

### 4.2 Ordering Context Language

| Term           | Meaning                           |
| :------------- | :-------------------------------- |
| **Order**      | Customer purchase request         |
| **OrderItem**  | Single product inside an order    |
| **Checkout**   | Confirmation process for an order |
| **TotalPrice** | Final order amount                |
| **Status**     | Current state of an order         |

### 4.3 Customer Context Language

| Term                    | Meaning                     |
| :---------------------- | :-------------------------- |
| **Customer**            | Person placing an order     |
| **Contact Information** | Customer communication data |
| **Email**               | Customer email address      |
| **Phone Number**        | Customer contact number     |

### 4.4 Reservation Context Language

| Term                  | Meaning                     |
| :-------------------- | :-------------------------- |
| **Reservation**       | Booking operation           |
| **GameSession**       | Organized gameplay activity |
| **Space**             | Physical table or room      |
| **ReservationStatus** | Current reservation state   |
| **SessionDate**       | Date of gameplay            |

---

## 5. Conclusion 🏁

During this laboratory work, the `BoardGameShop` project was analyzed from a domain-oriented perspective instead of a purely database-oriented structure. Domain events, commands, aggregates, bounded contexts, and ubiquitous language were identified for the system.

The analysis demonstrated that even a relatively simple e-commerce application contains multiple business boundaries and semantic contexts. The transition from classical layered architecture toward **Domain-Driven Design** allows better separation of responsibilities and improves understanding of business processes.

The resulting domain model can serve as a foundation for further migration toward a more complete DDD-oriented architecture in future project iterations.
