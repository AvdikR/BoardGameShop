# 📝 Practical Work №4: Implementing Tactical DDD Patterns

* **Discipline:** Domain Engineering Technologies
* **Project:** BoardGameShop
* **Theme:** Value Objects, Aggregate Roots, Domain Events and Use Cases
* **Repository:** https://github.com/AvdikR/BoardGameShop
* **Branch:** `feature/lab4-refactor`

---

# 1. Загальний опис

Дана практична робота продовжує перехід від класичної шарової архітектури до підходу Domain-Driven Design (DDD). На попередніх етапах було визначено доменну модель, агрегати, обмежені контексти та універсальну мову предметної області. Метою цієї роботи було перенесення бізнес-логіки з рівня сервісів у доменний шар та реалізація тактичних патернів DDD.

Реалізація виконувалась у межах існуючого проєкту `BoardGameShop`, який представляє онлайн-магазин настільних ігор, створений на основі ASP.NET Core, Entity Framework Core та .NET 10.

---

# 2. Виконані зміни

Під час рефакторингу було виконано такі зміни:

- додано Value Object `Money`;
- створено базовий клас `AggregateRoot`;
- перетворено `Order` на Aggregate Root;
- додано підтримку Domain Events;
- реалізовано подію `OrderCreatedEvent`;
- додано абстракцію `IDomainEvent`;
- розширено `BaseEntity` підтримкою доменних подій;
- реалізовано `CreateOrderCommandHandler`;
- додано транзакційність через `UnitOfWork`;
- бізнес-логіку перенесено всередину доменних сутностей.

---

# 3. Крок 1 – Реалізація Value Objects

## Призначення

Value Object використовується для представлення значень, які не мають власної ідентичності, але містять бізнес-правила.

У проєкті таким об’єктом є `Money`, який відповідає за роботу з грошовими значеннями.

---

## Реалізація Money

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
                "Сума не може бути від’ємною.");
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

---

## Результат

Value Object забезпечує:

- заборону від’ємних значень;
- незмінність об’єкта;
- централізовану бізнес-логіку;
- покращення читабельності доменної моделі.

---

# 4. Крок 2 – Перетворення Order на Aggregate Root

## Базовий клас AggregateRoot

Було введено базовий клас для агрегатів:

```csharp
public abstract class AggregateRoot : BaseEntity
{
}
```

---

## Агрегат Order

Сутність `Order` було перетворено на агрегат, який керує власним життєвим циклом.

```csharp
public class Order : AggregateRoot
{
    private readonly List<OrderItem> _orderItems = new();

    public IReadOnlyCollection<OrderItem> OrderItems
        => _orderItems.AsReadOnly();

    public OrderStatus Status { get; private set; }

    public Money TotalPrice { get; private set; }
}
```

---

## Бізнес-операції агрегату

Замість зміни властивостей напряму використовуються бізнес-методи.

### Додавання товару

```csharp
public void AddItem(Product product, int quantity)
{
    if (quantity <= 0)
    {
        throw new DomainException(
            "Кількість має бути більшою за 0.");
    }

    _orderItems.Add(
        new OrderItem(
            product.Id,
            quantity,
            product.Price));

    RecalculateTotal();
}
```

---

### Підтвердження замовлення

```csharp
public void Confirm()
{
    if (!_orderItems.Any())
    {
        throw new DomainException(
            "Неможливо підтвердити порожнє замовлення.");
    }

    Status = OrderStatus.Confirmed;
}
```

---

## Підтримувані операції

- `AssignCustomer()`
- `Pay()`
- `Ship()`
- `Deliver()`
- `Cancel()`

---

# 5. Крок 3 – Налаштування Domain Events Dispatcher

## Інтерфейс подій

```csharp
public interface IDomainEvent
{
}
```

---

## Підтримка у BaseEntity

```csharp
private readonly List<IDomainEvent> _domainEvents = new();

public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

protected void RaiseDomainEvent(IDomainEvent domainEvent)
{
    _domainEvents.Add(domainEvent);
}

public void ClearDomainEvents()
{
    _domainEvents.Clear();
}
```

---

## Подія створення замовлення

```csharp
public sealed class OrderCreatedEvent : IDomainEvent
{
    public int OrderId { get; }

    public OrderCreatedEvent(int orderId)
    {
        OrderId = orderId;
    }
}
```

---

## Виклик події

```csharp
RaiseDomainEvent(
    new OrderCreatedEvent(Id));
```

---

# 6. Крок 4 – Реалізація Use Case (Command Handler)

Для реалізації сценарію створення замовлення використано Command Handler.

## CreateOrderCommandHandler

```csharp
public class CreateOrderCommandHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<int> Handle(CreateOrderCommand command)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var customer =
                await _customerRepository.GetByIdAsync(command.CustomerId);

            var order = new Order();

            order.AssignCustomer(customer);

            foreach (var item in command.Items)
            {
                var product =
                    await _productRepository.GetByIdAsync(item.ProductId);

                order.AddItem(product, item.Quantity);
            }

            await _orderRepository.AddAsync(order);

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

# 7. Архітектурні зміни після рефакторингу

```
Application Layer
│
├── Commands (UseCases/Order)
│   └── CreateOrderCommandHandler
│
Domain Layer
│
├── Aggregates (Entities)
│   └── Order
│
├── ValueObjects (Common)
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

# 8. Висновок

У ході виконання практичної роботи було здійснено рефакторинг проєкту `BoardGameShop` із застосуванням тактичних патернів Domain-Driven Design.

Було реалізовано Value Object `Money`, який інкапсулює правила роботи з грошовими значеннями. При цьому було також досліджено інші види Value Objects, які можливо створити в проєкті. Сутність `Order` перетворено на Aggregate Root, який відповідає за цілісність бізнес-операцій. Додано підтримку доменних подій для фіксації важливих бізнес-дій, а також реалізовано Command Handler для сценарію створення замовлення.

У результаті доменна модель стала більш виразною, інкапсульованою та наближеною до реальних бізнес-процесів, що відповідає принципам Domain-Driven Design.
