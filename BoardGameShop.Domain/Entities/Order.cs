using BoardGameShop.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Domain.Entities
{
    public enum OrderStatus
    {
        Created,
        Paid,
        Cancelled
    }

    public class Order : BaseEntity
    {
        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();
    }
}
