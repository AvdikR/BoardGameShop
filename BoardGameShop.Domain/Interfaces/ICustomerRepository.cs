using BoardGameShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        
        Task<Customer?> GetByIdAsync(int id);

        Task AddAsync(Customer customer);
    }
}
