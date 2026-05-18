using BoardGameShop.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public int CustomerId { get; set; }
        public DateTime ReservationDate { get; set; }
    }
}
