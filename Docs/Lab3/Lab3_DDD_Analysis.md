# 📝 Laboratory Work №3: Domain Analysis and Transition from Layered Architecture to DDD

![.NET 10](https://img.shields.io/badge/.NET-10-512BD4?style=flat-square&logo=dotnet)
![Architecture](https://img.shields.io/badge/Architecture-DDD%20%2F%20Layered-blue?style=flat-square)

* **Discipline:** Domain Engineering Technologies  
* **Project:** `BoardGameShop`  
* **Theme:** Transition from Classical Layered Architecture to Domain-Driven Design  
* **Repository:** [GitHub: BoardGameShop Repository](https://github.com/AvdikR/BoardGameShop)  

---

## 1. Project Overview

The project represents an online board game store implemented as a RESTful Web API application using **.NET 10** and classical layered architecture principles. The system currently focuses on the core e-commerce workflow: browsing products, selecting games, creating orders, and confirming purchases.

> [!IMPORTANT]
> At the current stage, the project **does not implement authentication or authorization** functionality. Instead, customer information is entered during order creation. This approach allows the development team to focus primarily on business logic, domain modeling, and architectural organization.

> [!NOTE]
> The domain also includes planned extensions related to board game reservations, DnD sessions, and renting physical spaces for gameplay.

---

## 2. Stage 1 — Domain Events, Commands, and Aggregates

### 2.1 Domain Events 🔔

Domain events describe important business situations that already happened in the system.

#### 📦 Catalog Context
* `ProductCreated`
* `ProductUpdated`
* `ProductRemoved`
* `ProductAddedToCatalog`
* `ProductStockUpdated`

#### 🛒 Ordering Context
* `OrderCreated`
* `ProductAddedToOrder`
* `ProductRemovedFromOrder`
* `OrderConfirmed`
* `OrderCancelled`
* `OrderTotalCalculated`

#### 👥 Customer Context
* `CustomerInformationProvided`
* `CustomerDataUpdated`

#### 📅 Reservation Context
* `ReservationCreated`
* `ReservationCancelled`
* `ReservationCompleted`
* `GameSessionReserved`
* `SpaceReserved`

---

### 2.2 Commands ⚙️

Commands represent actions initiated by a user or the system that lead to domain events.

#### 📦 Catalog Commands
* `CreateProduct`
* `UpdateProduct`
* `DeleteProduct`
* `AddProductToCatalog`
* `UpdateProductStock`

#### 🛒 Ordering Commands
* `CreateOrder`
* `AddProductToOrder`
* `RemoveProductFromOrder`
* `ConfirmOrder`
* `CancelOrder`
* `CalculateOrderTotal`

#### 👥 Customer Commands
* `ProvideCustomerInformation`
* `UpdateCustomerInformation`

#### 📅 Reservation Commands
* `CreateReservation`
* `CancelReservation`
* `ReserveGameSession`
* `ReserveSpace`

---

### 2.3 Aggregates 🗃️

Aggregates group related business operations and protect consistency boundaries.

* **`Product` Aggregate**  
  Responsible for product information, pricing, stock quantity, preorder availability, and catalog assignment. All operations related to changing product data are performed through this aggregate.
* **`Order` Aggregate**  
  Controls the order lifecycle and ensures consistency of order items and total price calculation. The aggregate includes `Order` and `OrderItem` entities.
* **`Customer` Aggregate**  
  Manages customer contact information used during order confirmation and reservation creation.
* **`Reservation` Aggregate**  
  Responsible for booking operations related to game sessions and physical spaces.

---

## 3. Stage 2 — Bounded Contexts

The system was divided into several bounded contexts according to business responsibilities and semantic boundaries.

### 3.1 Catalog Context 📦

The Catalog context is responsible for storing and managing product information visible to customers. Inside this context, a `Product` represents a commercial item with description, price, category, and preorder information.

#### Responsibilities
- [x] Product management
- [x] Product categorization
- [x] Catalog browsing
- [x] Stock visibility

#### Main Terms
* `Product` | `Catalog` | `Category` | `Price` | `Stock`

---

### 3.2 Ordering Context 🛒

The Ordering context handles customer purchases and order processing. In this context, an `Order` represents a customer purchase process containing multiple order items.

#### Responsibilities
- [x] Order creation
- [x] Adding products to orders
- [x] Calculating totals
- [x] Order confirmation
- [x] Order status management

#### Main Terms
* `Order` | `OrderItem` | `Checkout` | `TotalPrice` | `Status`

---

### 3.3 Customer Context 👥

The Customer context is responsible for storing temporary customer contact information used during order confirmation. Since the system currently does not support authentication, the customer entity acts mainly as a data holder for purchase operations.

#### Responsibilities
- [x] Storing customer data
- [x] Updating customer information
- [x] Linking customers with orders

#### Main Terms
* `Customer` | `Contact Information` | `Email` | `Phone Number`

---

### 3.4 Reservation Context 📅

The Reservation context supports future functionality related to reserving spaces and game sessions. Inside this context, reservations represent booking operations rather than commercial purchases.

#### Responsibilities
- [x] Reservation management
- [x] Booking game sessions
- [x] Reserving physical spaces
- [x] Tracking reservation status

#### Main Terms
* `Reservation` | `GameSession` | `Space` | `ReservationStatus` | `SessionDate`

---

## 4. Stage 3 — Ubiquitous Language

A ubiquitous language was defined for each bounded context in order to ensure consistent communication between developers and domain experts.

### 4.1 Catalog Context Language

| Term | Meaning |
| :--- | :--- |
| **`Product`** | A board game or related item available in the store |
| **`Catalog`** | Collection of products grouped by categories |
| **`Category`** | Product grouping type |
| **`Price`** | Cost of a product |
| **`Stock`** | Available amount of products |
| **`Preorder`** | Ability to purchase a product before release |
| **`Description`** | Product information visible to customers |

### 4.2 Ordering Context Language

| Term | Meaning |
| :--- | :--- |
| **`Order`** | Customer purchase request |
| **`OrderItem`** | Single product inside an order |
| **`Checkout`** | Confirmation process for an order |
| **`TotalPrice`** | Final order amount |
| **`Status`** | Current state of an order |
| **`Cart`** | Temporary collection of selected products |
| **`Confirmation`** | Finalization of an order |

### 4.3 Customer Context Language

| Term | Meaning |
| :--- | :--- |
| **`Customer`** | Person placing an order |
| **`Contact Information`** | Customer communication data |
| **`Email`** | Customer email address |
| **`Phone Number`** | Customer contact number |
| **`Full Name`** | Customer identification information |

### 4.4 Reservation Context Language

| Term | Meaning |
| :--- | :--- |
| **`Reservation`** | Booking operation |
| **`GameSession`** | Organized gameplay activity |
| **`Space`** | Physical table or room |
| **`ReservationStatus`** | Current reservation state |
| **`SessionDate`** | Date of gameplay |
| **`Duration`** | Gameplay length |
| **`Capacity`** | Maximum number of participants |

---

## 5. Conclusion 🏁

During this laboratory work, the `BoardGameShop` project was analyzed from a domain-oriented perspective instead of a purely database-oriented structure. Domain events, commands, aggregates, bounded contexts, and ubiquitous language were identified for the system.

The analysis demonstrated that even a relatively simple e-commerce application contains multiple business boundaries and semantic contexts. The transition from classical layered architecture toward **Domain-Driven Design** allows better separation of responsibilities and improves understanding of business processes.

The resulting domain model can serve as a foundation for further migration toward a more complete DDD-oriented architecture in future project iterations.
