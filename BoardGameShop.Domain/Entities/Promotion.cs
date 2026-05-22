using System;

namespace BoardGameShop.Domain.Entities
{
    public class Promotion
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime ActiveFrom { get; set; }

        public DateTime ActiveTo { get; set; }

        // If null, applies to all products; otherwise applies to specific catalog/category
        public int? CatalogId { get; set; }

        // Percentage discount (0.10 = 10%)
        public decimal Percentage { get; set; }
    }
}
