using System;

namespace BoardGameShop.Domain.Common
{
    // Value Object representing a discount percentage (0..100)
    public sealed record Discount
    {
        public decimal Percentage { get; init; }

        protected Discount() { }

        private Discount(decimal percentage)
        {
            Percentage = percentage;
        }

        public static Discount FromPercent(decimal percent)
        {
            if (percent < 0 || percent > 100)
                throw new DomainException("Discount percent must be between 0 and 100");

            return new Discount(percent);
        }

        public decimal Apply(decimal amount)
        {
            return amount - (amount * (Percentage / 100m));
        }
    }
}
