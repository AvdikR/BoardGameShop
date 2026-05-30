using BoardGameShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BoardGameShop.Infrastructure.Data
{
    public class BoardGameShopDbContext : DbContext
    {
        public BoardGameShopDbContext(DbContextOptions<BoardGameShopDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<Catalog> Catalogs => Set<Catalog>();
        public DbSet<BoardGameShop.Domain.Entities.Promotion> Promotions => Set<BoardGameShop.Domain.Entities.Promotion>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            modelBuilder.Entity<Order>()
                .OwnsOne(o => o.TotalPrice, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("TotalPriceAmount").HasPrecision(18, 2);
                    money.Property(m => m.Currency).HasColumnName("TotalPriceCurrency").HasMaxLength(3);
                });
        }


}

}
