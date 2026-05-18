using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Application.DTOs
{
    public class CreateCustomerDto
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
