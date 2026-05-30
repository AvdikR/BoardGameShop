using System;

namespace BoardGameShop.Domain.Common
{
    public record Money
    {
        public decimal Amount { get; init; }
        public string Currency { get; init; }

        protected Money() { }

        public Money(decimal amount, string currency = "UAH")
        {
            if (amount < 0)
                throw new DomainException("Сума не може бути від'ємною");

            Amount = amount;
            Currency = currency;
        }

        public static Money Zero(string currency = "UAH") => new Money(0, currency);
    }
}