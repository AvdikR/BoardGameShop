using BoardGameShop.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }

        public Order Order { get; set; } = null!;

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
