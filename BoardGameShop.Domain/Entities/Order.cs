using BoardGameShop.Domain.Common;
using BoardGameShop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoardGameShop.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int CustomerId { get; private set; }

        public Customer Customer { get; private set; } = null!;

        public DateTime OrderDate { get; private set; }

        public decimal TotalPrice { get; private set; }

        public OrderStatus Status { get; private set; }

        public ICollection<OrderItem> OrderItems { get; private set; }
            = new List<OrderItem>();

        // ---------------------------------
        // Constructor
        // ---------------------------------

        private Order() { }

        public Order(int customerId)
        {
            CustomerId = customerId;

            OrderDate = DateTime.UtcNow;
            Status = OrderStatus.Created;
        }

        // ---------------------------------
        // DOMAIN METHODS
        // ---------------------------------

        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Quantity must be greater than 0");

            // Inventory rule
            product.ReserveStock(quantity);

            var existingItem = OrderItems
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

                OrderItems.Add(orderItem);
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

            TotalPrice = Domain.Services.PricingCalculator
                .CalculateTotal(OrderItems, loyalty);
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
            if (!OrderItems.Any())
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
    }
}
