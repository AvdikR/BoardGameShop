using BoardGameShop.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameShop.Application.Interfaces
{
    public interface IProductService
    {
        Task CreateAsync(CreateProductDto dto);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task UpdateAsync(int id, UpdateProductDto dto);
        Task DeleteAsync(int id);
        Task IncreaseStockAsync(int id, int quantity);
        Task ReserveStockAsync(int id, int quantity);
    }
}
