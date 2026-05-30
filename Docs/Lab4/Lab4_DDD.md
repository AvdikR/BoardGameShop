# 📝 Practical Work №4: Implementing Tactical DDD Patterns

* **Discipline:** Domain Engineering Technologies
* **Project:** BoardGameShop
* **Theme:** Value Objects, Aggregate Roots, Domain Events and Use Cases
* **Repository:** https://github.com/AvdikR/BoardGameShop
* **Branch:** `feature/lab4-refactor`

---

# 1. Overview

This practical work continues the transition from a classical layered architecture toward Domain-Driven Design (DDD). During previous stages, the domain model, bounded contexts, aggregates and ubiquitous language were identified. The goal of this work was to move business rules from service and infrastructure layers into the domain layer and implement tactical DDD patterns.

The implementation was performed on the existing `BoardGameShop` project, which represents an online board game store built with ASP.NET Core, Entity Framework Core and .NET 10.

---

# 2. Implemented Changes

During the refactoring process the following improvements were introduced:

* Added `Money` Value Object.
* Introduced `AggregateRoot` base class.
* Converted `Order` into an Aggregate Root.
* Added support for Domain Events.
* Implemented `OrderCreatedEvent`.
* Added `IDomainEvent` abstraction.
* Extended `BaseEntity` with domain event support.
* Implemented `CreateOrderCommandHandler`.
* Integrated transaction management through `UnitOfWork`.
* Moved business validation rules into domain entities.

---

# 3. Step 1 – Value Objects

## Motivation

Originally prices were represented using primitive data types (`decimal`). Such an approach scatters validation logic throughout the application.

To encapsulate money-related rules, a dedicated `Money` Value Object was introduced.

## Money.cs

```csharp
public sealed record Money
{
    public decimal Amount { get; init; }

    public string Currency { get; init; }

    public static Money Create(
        decimal amount,
        string currency = "UAH")
    {
        if (amount < 0)
        {
            throw new DomainException(
                "Amount cannot be negative.");
        }

        return new Money
        {
            Amount = amount,
            Currency = currency
        };
    }

    public static Money Zero(
        string currency = "UAH")
    {
        return new Money
        {
            Amount = 0,
            Currency = currency
        };
    }
}
```

## Result

The Value Object guarantees:

* non-negative monetary values;
* immutable representation of money;
* centralized validation;
* improved expressiveness of the domain model.

---

# 4. Step 2 – Aggregate Root

## AggregateRoot Base Class

A dedicated Aggregate Root abstraction was introduced to support domain events and aggregate consistency boundaries.

```csharp
public abstract class AggregateRoot : BaseEntity
{
}
```

---

## Order Aggregate

The `Order` entity was transformed into an Aggregate Root.

```csharp
public class Order : AggregateRoot
{
    private readonly List<OrderItem> _orderItems = new();

    public IReadOnlyCollection<OrderItem> OrderItems
        => _orderItems.AsReadOnly();

    public OrderStatus Status { get; private set; }

    public Money TotalPrice { get; private set; }

    // Business methods
}
```

---

## Business Operations

Instead of exposing setters, the aggregate now controls state changes through business methods.

### Adding Items

```csharp
public void AddItem(
    Product product,
    int quantity)
{
    if (quantity <= 0)
    {
        throw new DomainException(
            "Quantity must be positive.");
    }

    _orderItems.Add(
        new OrderItem(
            product.Id,
            quantity,
            product.Price));

    RecalculateTotal();
}
```

### Confirming Orders

```csharp
public void Confirm()
{
    if (!_orderItems.Any())
    {
        throw new DomainException(
            "Cannot confirm an empty order.");
    }

    Status = OrderStatus.Confirmed;
}
```

### Other Supported Operations

* `AssignCustomer()`
* `Pay()`
* `Ship()`
* `Deliver()`
* `Cancel()`

---

# 5. Step 3 – Domain Events

## Domain Event Abstraction

```csharp
public interface IDomainEvent
{
}
```

---

## BaseEntity Support

Domain events are stored inside the base entity.

```csharp
private readonly List<IDomainEvent> _domainEvents
    = new();

public IReadOnlyCollection<IDomainEvent>
    DomainEvents => _domainEvents;

protected void RaiseDomainEvent(
    IDomainEvent domainEvent)
{
    _domainEvents.Add(domainEvent);
}

public void ClearDomainEvents()
{
    _domainEvents.Clear();
}
```

---

## OrderCreatedEvent

```csharp
public sealed class OrderCreatedEvent
    : IDomainEvent
{
    public int OrderId { get; }

    public OrderCreatedEvent(int orderId)
    {
        OrderId = orderId;
    }
}
```

---

## Raising Events

The event is generated automatically during order creation.

```csharp
RaiseDomainEvent(
    new OrderCreatedEvent(Id));
```

---

# 6. Step 4 – Use Case Implementation

To orchestrate the order creation workflow, a Command Handler was implemented.

## CreateOrderCommandHandler

```csharp
public class CreateOrderCommandHandler
{
    private readonly IOrderRepository
        _orderRepository;

    private readonly IProductRepository
        _productRepository;

    private readonly ICustomerRepository
        _customerRepository;

    private readonly IUnitOfWork
        _unitOfWork;

    public async Task<int> Handle(
        CreateOrderCommand command)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var customer =
                await _customerRepository
                    .GetByIdAsync(
                        command.CustomerId);

            var order =
                new Order();

            order.AssignCustomer(customer);

            foreach (var item in command.Items)
            {
                var product =
                    await _productRepository
                        .GetByIdAsync(item.ProductId);

                order.AddItem(
                    product,
                    item.Quantity);
            }

            await _orderRepository
                .AddAsync(order);

            await _unitOfWork.CommitAsync();

            return order.Id;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
```

---

# 7. Architecture After Refactoring

```text
Application Layer
│
├── Commands
│   └── CreateOrderCommandHandler
│
Domain Layer
│
├── Aggregates
│   └── Order
│
├── ValueObjects
│   └── Money
│
├── Events
│   └── OrderCreatedEvent
│
└── Common
    ├── AggregateRoot
    └── BaseEntity
│
Infrastructure Layer
│
├── Repositories
├── UnitOfWork
└── EF Core
```

---

# 8. Conclusion

During this practical work the `BoardGameShop` project was refactored using tactical Domain-Driven Design patterns.

A dedicated `Money` Value Object was introduced to encapsulate monetary rules. The `Order` entity became an Aggregate Root responsible for enforcing business invariants and controlling its own lifecycle. Domain Events were added to represent important business occurrences, while `CreateOrderCommandHandler` was implemented to orchestrate the order creation use case.

As a result, the domain model became more expressive, business-oriented and less dependent on infrastructure concerns. The project now follows DDD principles more closely and provides a solid foundation for future architectural evolution.
