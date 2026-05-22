using BoardGameShop.Domain.Entities;
using BoardGameShop.Domain.Interfaces;
using BoardGameShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameShop.Infrastructure.Repository
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly BoardGameShopDbContext _context;

        public PromotionRepository(BoardGameShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Promotion>> GetActivePromotionsAsync(DateTime at)
        {
            return await _context.Set<Promotion>()
                .Where(p => p.ActiveFrom <= at && p.ActiveTo >= at)
                .ToListAsync();
        }
    }
}
