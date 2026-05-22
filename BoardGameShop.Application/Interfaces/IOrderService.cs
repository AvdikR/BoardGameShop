using BoardGameShop.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameShop.Application.Interfaces
{
    public interface IOrderService
    {
        Task<int> CreateAsync(int customerId, List<(int productId, int quantity)> items);
        Task<int> CreateAsync(CreateOrderDto dto);
        Task<OrderDto?> GetByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task ConfirmAsync(int orderId);
        Task CancelAsync(int orderId);

    }
}
