using BoardGameShop.Domain.Common;
using BoardGameShop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoardGameShop.Domain.Entities
{
    public class Order : Common.AggregateRoot
    {
        public int CustomerId { get; private set; }

        public Customer Customer { get; private set; } = null!;

        public DateTime OrderDate { get; private set; }

        public Money TotalPrice { get; private set; } = Money.Zero();

        public OrderStatus Status { get; private set; }

        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        // ---------------------------------
        // Constructor
        // ---------------------------------

        private Order() { }

        // ---------------------------------
        // DOMAIN METHODS
        // ---------------------------------

        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Quantity must be greater than 0");

            // Inventory rule
            product.ReserveStock(quantity);

            var existingItem = _orderItems
                .FirstOrDefault(x => x.ProductId == product.Id);

            // Якщо товар вже є в замовленні
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                var orderItem = new OrderItem(
                    product.Id,
                    quantity,
                    product.Price
                );

                _orderItems.Add(orderItem);
            }

            RecalculateTotal();
        }

        public void AssignCustomer(Customer customer)
        {
            if (customer == null)
                throw new DomainException("Customer cannot be null");

            Customer = customer;
            CustomerId = customer.Id;

            // Recalculate price if items already present
            RecalculateTotal();
        }

        // ---------------------------------
        // BUSINESS RULE
        // ---------------------------------

        private void RecalculateTotal()
        {
            var loyalty = Customer != null ? Customer.LoyaltyTier : Domain.Enums.LoyaltyTier.Bronze;
            var totalDecimal = Domain.Services.PricingCalculator.CalculateTotal(_orderItems, loyalty);

            TotalPrice = Money.Create(totalDecimal); // Використовуємо Value Object
        }

        // Basic discount rules — keep small and explicit for lab requirements
        // - 15% off for orders >= 200
        // - 10% off for orders >= 100
        // - 5% off if total item quantity >= 10
        // Pricing logic moved to PricingCalculator for testability

        // ---------------------------------
        // WORKFLOW METHODS
        // ---------------------------------

        public void Confirm()
        {
            if (!_orderItems.Any())
                throw new DomainException("Order cannot be empty");
            if (Status != OrderStatus.Created)
                throw new DomainException("Only created orders can be confirmed");

            Status = OrderStatus.Confirmed;
        }

        public void Pay()
        {
            if (Status != OrderStatus.Confirmed)
                throw new DomainException("Only confirmed orders can be paid");

            Status = OrderStatus.Paid;
        }

        public void Ship()
        {
            if (Status != OrderStatus.Paid)
                throw new DomainException("Only paid orders can be shipped");

            Status = OrderStatus.Shipped;
        }

        public void Deliver()
        {
            if (Status != OrderStatus.Shipped)
                throw new DomainException("Only shipped orders can be delivered");

            Status = OrderStatus.Delivered;
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Cancelled)
                throw new DomainException("Order already cancelled");

            // Only allow cancel before payment/shipping
            if (Status != OrderStatus.Created && Status != OrderStatus.Confirmed)
                throw new DomainException("Only created or confirmed orders can be cancelled");

            Status = OrderStatus.Cancelled;
        }

        public Order(int customerId)
        {
            CustomerId = customerId;
            OrderDate = DateTime.UtcNow;
            Status = OrderStatus.Created;

            // Збуджуємо доменну подію
            RaiseDomainEvent(new Events.OrderCreatedEvent(this.Id, customerId));
        }
    }
}
