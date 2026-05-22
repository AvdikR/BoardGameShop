using System;
using System.Collections.Generic;
using System.Text;
using BoardGameShop.Domain.Entities;
using BoardGameShop.Domain.Interfaces;
using BoardGameShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BoardGameShop.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BoardGameShopDbContext _context;

        public OrderRepository(BoardGameShopDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Customer)
                .ToListAsync();
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            // SaveChanges managed by application service/unit of work
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            // SaveChanges managed by application service/unit of work
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
