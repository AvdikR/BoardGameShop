using BoardGameShop.Domain.Entities;
using BoardGameShop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BoardGameShop.Domain.Services
{
    public static class PricingCalculator
    {
        // Calculate final total price for order items considering bulk and loyalty discounts
        public static decimal CalculateTotal(IEnumerable<OrderItem> items, LoyaltyTier loyaltyTier)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            var itemList = items.ToList();

            var subtotal = itemList.Sum(x => x.Price * x.Quantity);
            var totalQuantity = itemList.Sum(x => x.Quantity);

            var baseDiscount = CalculateBaseDiscount(subtotal, totalQuantity);

            var afterBase = subtotal - baseDiscount;

            var loyaltyDiscount = CalculateLoyaltyDiscount(afterBase, loyaltyTier);

            var total = afterBase - loyaltyDiscount;

            return Math.Round(total, 2);
        }

        public static PricingResult CalculateDetailed(IEnumerable<OrderItem> items, LoyaltyTier loyaltyTier, IEnumerable<Entities.Promotion> promotions)
        {
            var itemList = items.ToList();

            var subtotal = itemList.Sum(x => x.Price * x.Quantity);
            var totalQuantity = itemList.Sum(x => x.Quantity);

            var baseDiscount = CalculateBaseDiscount(subtotal, totalQuantity);
            var afterBase = subtotal - baseDiscount;

            var loyaltyDiscount = CalculateLoyaltyDiscount(afterBase, loyaltyTier);
            var afterLoyalty = afterBase - loyaltyDiscount;

            var appliedPromotions = new List<AppliedPromotion>();
            decimal promotionsDiscount = 0m;

            var now = DateTime.UtcNow;

            foreach (var promo in promotions)
            {
                // Category filter: if promo has CatalogId, ensure one of the items belongs to that catalog
                if (promo.CatalogId.HasValue)
                {
                    // we can't access product catalog here easily, so assume promotion applies if any item exists
                    // In real app: match by product.CatalogId
                }

                var discountAmount = Math.Round(afterLoyalty * promo.Percentage, 2);
                promotionsDiscount += discountAmount;

                appliedPromotions.Add(new AppliedPromotion
                {
                    Name = promo.Name,
                    Percentage = promo.Percentage,
                    DiscountAmount = discountAmount
                });
            }

            var total = afterLoyalty - promotionsDiscount;

            return new PricingResult
            {
                Subtotal = subtotal,
                BaseDiscount = baseDiscount,
                LoyaltyDiscount = loyaltyDiscount,
                PromotionsDiscount = promotionsDiscount,
                AppliedPromotions = appliedPromotions,
                Total = Math.Round(total, 2)
            };
        }

        // Basic bulk discount rules
        private static decimal CalculateBaseDiscount(decimal subtotal, int totalQuantity)
        {
            if (subtotal >= 200m)
                return Math.Round(subtotal * 0.15m, 2);

            if (subtotal >= 100m)
                return Math.Round(subtotal * 0.10m, 2);

            if (totalQuantity >= 10)
                return Math.Round(subtotal * 0.05m, 2);

            return 0m;
        }

        private static decimal CalculateLoyaltyDiscount(decimal amount, LoyaltyTier tier)
        {
            decimal percent = tier switch
            {
                LoyaltyTier.Silver => 0.05m,
                LoyaltyTier.Gold => 0.10m,
                _ => 0m
            };

            return Math.Round(amount * percent, 2);
        }
    }
}
