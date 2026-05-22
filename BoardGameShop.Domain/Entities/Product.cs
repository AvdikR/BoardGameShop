using BoardGameShop.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public decimal Price { get; private set; }

        public int StockQuantity { get; private set; }

        public int CatalogId { get; private set; }

        public Catalog Catalog { get; private set; } = null!;

        private Product() { }

        public Product(
            string name,
            string description,
            decimal price,
            int stockQuantity,
            int catalogId)
        {
            Name = name;

            Description = description;

            Price = price;

            StockQuantity = stockQuantity;

            CatalogId = catalogId;
        }

        // ---------------------------------
        // STOCK METHODS
        // ---------------------------------

        public void ReserveStock(int quantity)
        {
            if (quantity <= 0)
                throw new Exception(
                    "Quantity must be greater than 0"
                );

            if (StockQuantity < quantity)
                throw new Exception(
                    "Not enough stock"
                );

            StockQuantity -= quantity;
        }

        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new Exception(
                    "Quantity must be greater than 0"
                );

            StockQuantity += quantity;
        }

        // ---------------------------------
        // UPDATE PRODUCT
        // ---------------------------------

        public void UpdateInfo(
            string name,
            string description,
            decimal price)
        {
            Name = name;

            Description = description;

            Price = price;
        }
    }
}
