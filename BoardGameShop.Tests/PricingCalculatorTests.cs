using System.Collections.Generic;
using Xunit;
using BoardGameShop.Domain.Services;
using BoardGameShop.Domain.Entities;
using BoardGameShop.Domain.Enums;

namespace BoardGameShop.Tests
{
    public class PricingCalculatorTests
    {
        [Fact]
        public void CalculateTotal_AppliesBaseBulkDiscounts()
        {
            var items = new List<OrderItem>
            {
                new OrderItem(1, 2, 60m), // 120
                new OrderItem(2, 1, 40m)  // 40 -> subtotal 160 -> 10% discount = 16 -> 144
            };

            var total = PricingCalculator.CalculateTotal(items, LoyaltyTier.Bronze);

            Assert.Equal(144m, total);
        }

        [Fact]
        public void CalculateTotal_AppliesLoyaltyDiscount()
        {
            var items = new List<OrderItem>
            {
                new OrderItem(1, 1, 100m)
            };

            var total = PricingCalculator.CalculateTotal(items, LoyaltyTier.Gold);

            // subtotal 100 -> base discount 10% = 10 -> afterBase = 90
            // loyalty 10% of 90 = 9 -> total 81
            Assert.Equal(81m, total);
        }

        [Fact]
        public void CalculateDetailed_IncludesPromotions()
        {
            var items = new List<OrderItem>
            {
                new OrderItem(1, 1, 200m)
            };

            var promotions = new List<Promotion>
            {
                new Promotion { Name = "Summer", ActiveFrom = System.DateTime.UtcNow.AddDays(-1), ActiveTo = System.DateTime.UtcNow.AddDays(1), Percentage = 0.10m }
            };

            var result = PricingCalculator.CalculateDetailed(items, LoyaltyTier.Bronze, promotions);

            // subtotal 200 -> base discount 15% = 30 -> afterBase 170
            // loyalty 0 -> afterLoyalty 170
            // promotion 10% of 170 = 17 -> total 153
            Assert.Equal(153m, result.Total);
            Assert.Single(result.AppliedPromotions);
            Assert.Equal(17m, result.PromotionsDiscount);
        }
    }
}
