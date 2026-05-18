using BoardGameShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameShop.Domain.Interfaces
{
    public interface ICatalogRepository
    {
        Task<IEnumerable<Catalog>> GetAllAsync();

        Task<Catalog?> GetByIdAsync(int id);

        Task AddAsync(Catalog catalog);
    }
}
