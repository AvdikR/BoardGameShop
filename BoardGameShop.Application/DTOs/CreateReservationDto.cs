using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Application.DTOs
{
    public class CreateReservationDto
    {
        public int CustomerId { get; set; }

        public string GameSessionName { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public int DurationHours { get; set; }
    }
}
