using BoardGameShop.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Application.Interfaces
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogDto>> GetAllAsync();

        Task<CatalogDto?> GetByIdAsync(int id);

        Task CreateAsync(CreateCatalogDto dto);
    }
}
