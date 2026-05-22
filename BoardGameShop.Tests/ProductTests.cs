using System;
using Xunit;
using BoardGameShop.Domain.Entities;
using BoardGameShop.Domain.Common;

namespace BoardGameShop.Tests
{
    public class ProductTests
    {
        [Fact]
        public void IncreaseStock_ValidQuantity_AddsToStock()
        {
            var product = new Product("Catan", "Базова гра", 1500m, 10, 1);

            product.IncreaseStock(5);

            Assert.Equal(15, product.StockQuantity);
        }

        [Fact]
        public void ReserveStock_ValidQuantity_ReducesStock()
        {
            var product = new Product("Monopoly", "Класична", 1000m, 20, 1);

            product.ReserveStock(5);

            Assert.Equal(15, product.StockQuantity);
        }

        [Fact]
        public void ReserveStock_ExceedsAvailable_ThrowsDomainException()
        {
            var product = new Product("Munchkin", "Карткова", 500m, 2, 1);

            Assert.Throws<Exception>(() => product.ReserveStock(10));
        }
    }
}