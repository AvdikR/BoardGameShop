using BoardGameShop.Domain.Common;
using BoardGameShop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public string GameSessionName { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public int DurationHours { get; set; }

        public ReservationStatus Status { get; set; }
    }
}
