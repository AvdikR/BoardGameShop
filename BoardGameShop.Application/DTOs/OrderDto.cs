using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = string.Empty;

        public List<OrderItemDto> Items { get; set; }
            = new();
    }
}
