using System.Collections.Generic;

namespace BoardGameShop.Domain.Services
{
    public class AppliedPromotion
    {
        public string Name { get; set; } = string.Empty;

        public decimal Percentage { get; set; }

        public decimal DiscountAmount { get; set; }
    }

    public class PricingResult
    {
        public decimal Subtotal { get; set; }

        public decimal BaseDiscount { get; set; }

        public decimal LoyaltyDiscount { get; set; }

        public decimal PromotionsDiscount { get; set; }

        public List<AppliedPromotion> AppliedPromotions { get; set; } = new();

        public decimal Total { get; set; }
    }
}
