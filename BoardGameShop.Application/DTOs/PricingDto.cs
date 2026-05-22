using System;
using System.Collections.Generic;

namespace BoardGameShop.Application.DTOs
{
    public class AppliedPromotionDto
    {
        public string Name { get; set; } = string.Empty;

        public decimal Percentage { get; set; }

        public decimal DiscountAmount { get; set; }
    }

    public class PricingDto
    {
        public decimal Subtotal { get; set; }

        public decimal BaseDiscount { get; set; }

        public decimal LoyaltyDiscount { get; set; }

        public decimal PromotionsDiscount { get; set; }

        public List<AppliedPromotionDto> AppliedPromotions { get; set; } = new();

        public decimal Total { get; set; }
    }
}
