using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Domain.Enums
{
    public enum OrderStatus
    {
        Created,
        Confirmed,
        Paid,
        Shipped,
        Delivered,
        Cancelled
    }
}
