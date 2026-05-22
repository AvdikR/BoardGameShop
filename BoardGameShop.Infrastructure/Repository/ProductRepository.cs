using BoardGameShop.Domain.Entities;
using BoardGameShop.Domain.Interfaces;
using BoardGameShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameShop.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly BoardGameShopDbContext _context;

        public ProductRepository(BoardGameShopDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            // SaveChanges is managed by the application service (unit of work)
            return;
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            // SaveChanges is managed by the application service (unit of work)
            return;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                // SaveChanges is managed by the application service (unit of work)
                return;
            }
        }

    }
}
