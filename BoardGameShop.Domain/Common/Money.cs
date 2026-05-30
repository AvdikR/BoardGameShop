using System;

namespace BoardGameShop.Domain.Common
{
    // Value Object representing money/price
    public sealed record Money
    {
        public decimal Amount { get; init; }
        public string Currency { get; init; }

        // EF Core requires a parameterless constructor for owned types
        protected Money() { }

        private Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public static Money Create(decimal amount, string currency = "UAH")
        {
            if (amount < 0)
                throw new DomainException("Сума не може бути від'ємною");

            return new Money(amount, currency);
        }

        public static Money Zero(string currency = "UAH") => Create(0, currency);
    }
}
