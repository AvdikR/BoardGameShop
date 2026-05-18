using BoardGameShop.Domain.Entities;
using BoardGameShop.Domain.Interfaces;
using BoardGameShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BoardGameShop.Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BoardGameShopDbContext _context;

        public CustomerRepository(BoardGameShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }
    }
}
