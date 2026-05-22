using BoardGameShop.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; private set; }

        public Order Order { get; private set; } = null!;

        public int ProductId { get; private set; }

        public Product Product { get; private set; } = null!;

        public int Quantity { get; private set; }

        public decimal Price { get; private set; }

        private OrderItem() { }

        public OrderItem(
            int productId,
            int quantity,
            decimal price)
        {
            ProductId = productId;

            Quantity = quantity;

            Price = price;
        }

        public void IncreaseQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new Exception(
                    "Quantity must be greater than 0"
                );

            Quantity += quantity;
        }
    }
}
