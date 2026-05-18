using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Application.DTOs
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
