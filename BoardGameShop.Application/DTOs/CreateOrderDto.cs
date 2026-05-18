using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Application.DTOs
{
    public class CreateOrderDto
    {
        public int CustomerId { get; set; }

        public List<OrderItemDto> Items { get; set; }
            = new();
    }
}
