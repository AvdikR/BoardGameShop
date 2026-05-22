using BoardGameShop.Domain.Common;
using BoardGameShop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;


namespace BoardGameShop.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public LoyaltyTier LoyaltyTier { get; private set; } = LoyaltyTier.Bronze;

    }
}
