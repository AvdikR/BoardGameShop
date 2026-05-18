using BoardGameShop.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllAsync();

        Task<CustomerDto?> GetByIdAsync(int id);

        Task CreateAsync(CreateCustomerDto dto);
    }
}
