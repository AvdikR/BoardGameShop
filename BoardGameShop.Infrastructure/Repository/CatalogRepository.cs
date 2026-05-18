using BoardGameShop.Domain.Entities;
using BoardGameShop.Domain.Interfaces;
using BoardGameShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BoardGameShop.Infrastructure.Repository
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly BoardGameShopDbContext _context;

        public CatalogRepository(BoardGameShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Catalog>> GetAllAsync()
        {
            return await _context.Catalogs
                .Include(c => c.Products)
                .ToListAsync();
        }

        public async Task<Catalog?> GetByIdAsync(int id)
        {
            return await _context.Catalogs
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Catalog catalog)
        {
            await _context.Catalogs.AddAsync(catalog);
            await _context.SaveChangesAsync();
        }
    }
}
