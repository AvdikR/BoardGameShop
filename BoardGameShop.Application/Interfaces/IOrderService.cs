using BoardGameShop.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();

        Task<OrderDto?> GetByIdAsync(int id);

        Task CreateAsync(CreateOrderDto dto);
    }
}
