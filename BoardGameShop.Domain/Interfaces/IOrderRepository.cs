using System;
using System.Collections.Generic;
using System.Text;
using BoardGameShop.Domain.Entities;

namespace BoardGameShop.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();

        Task<Order?> GetByIdAsync(int id);

        Task AddAsync(Order order);

        Task UpdateAsync(Order order);

        // Commit pending changes to the database (Unit of Work)
        Task SaveChangesAsync();
    }
}
