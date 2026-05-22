using System;
using Xunit;
using BoardGameShop.Domain.Entities;
using BoardGameShop.Domain.Common;
using BoardGameShop.Domain.Enums;

namespace BoardGameShop.Tests
{
    public class OrderWorkflowTests
    {
        [Fact]
        public void Confirm_WithItems_Succeeds()
        {
            var order = new Order(1);
            order.OrderItems.Add(new OrderItem(1, 1, 10m));

            order.Confirm();

            Assert.Equal(OrderStatus.Confirmed, order.Status);
        }

        [Fact]
        public void Confirm_WithoutItems_Throws()
        {
            var order = new Order(1);

            Assert.Throws<DomainException>(() => order.Confirm());
        }

        [Fact]
        public void FullWorkflow_AllTransitionsAllowed()
        {
            var order = new Order(1);
            order.OrderItems.Add(new OrderItem(1, 1, 10m));

            order.Confirm();
            order.Pay();
            order.Ship();
            order.Deliver();

            Assert.Equal(OrderStatus.Delivered, order.Status);
        }

        [Fact]
        public void Ship_WithoutPay_Throws()
        {
            var order = new Order(1);
            order.OrderItems.Add(new OrderItem(1, 1, 10m));

            order.Confirm();

            Assert.Throws<DomainException>(() => order.Ship());
        }

        [Fact]
        public void Cancel_FromCreatedAndConfirmed_Succeeds()
        {
            var order1 = new Order(1);
            // cancel directly from Created
            order1.Cancel();
            Assert.Equal(OrderStatus.Cancelled, order1.Status);

            var order2 = new Order(2);
            order2.OrderItems.Add(new OrderItem(2, 1, 20m));
            order2.Confirm();
            order2.Cancel();
            Assert.Equal(OrderStatus.Cancelled, order2.Status);
        }

        [Fact]
        public void Cancel_FromShipped_Throws()
        {
            var order = new Order(1);
            order.OrderItems.Add(new OrderItem(1, 1, 10m));

            order.Confirm();
            order.Pay();
            order.Ship();

            Assert.Throws<DomainException>(() => order.Cancel());
        }

        [Fact]
        public void Pay_WithoutConfirm_Throws()
        {
            var order = new Order(1);
            order.OrderItems.Add(new OrderItem(1, 1, 10m));

            Assert.Throws<DomainException>(() => order.Pay());
        }
    }
}
