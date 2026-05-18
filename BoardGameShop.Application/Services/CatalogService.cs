using System;
using System.Collections.Generic;
using System.Text;
using BoardGameShop.Application.DTOs;
using BoardGameShop.Application.Interfaces;
using BoardGameShop.Domain.Entities;
using BoardGameShop.Domain.Interfaces;

namespace BoardGameShop.Application.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _catalogRepository;

        public CatalogService(ICatalogRepository catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        // -----------------------------
        // GET ALL
        // -----------------------------
        public async Task<IEnumerable<CatalogDto>> GetAllAsync()
        {
            var catalogs = await _catalogRepository.GetAllAsync();

            return catalogs.Select(c => new CatalogDto
            {
                Id = c.Id,
                Name = c.Name
            });
        }

        // -----------------------------
        // GET BY ID
        // -----------------------------
        public async Task<CatalogDto?> GetByIdAsync(int id)
        {
            var catalog = await _catalogRepository.GetByIdAsync(id);

            if (catalog == null)
                return null;

            return new CatalogDto
            {
                Id = catalog.Id,
                Name = catalog.Name
            };
        }

        // -----------------------------
        // CREATE
        // -----------------------------
        public async Task CreateAsync(CreateCatalogDto dto)
        {
            var catalog = new Catalog
            {
                Name = dto.Name
            };

            await _catalogRepository.AddAsync(catalog);
        }
    }
}
