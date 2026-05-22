using BoardGameShop.Application.DTOs;
using BoardGameShop.Application.Interfaces;
using BoardGameShop.Domain.Entities;
using BoardGameShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameShop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // ---------------------------------
        // CREATE PRODUCT
        // ---------------------------------

        public async Task CreateAsync(CreateProductDto dto)
        {
            var product = new Product(
                dto.Name,
                dto.Description,
                dto.Price,
                dto.StockQuantity,
                dto.CatalogId
            );

            await _productRepository.AddAsync(product);
        }

        // ---------------------------------
        // GET ALL
        // ---------------------------------

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CatalogId = p.CatalogId
            });
        }

        // ---------------------------------
        // GET BY ID
        // ---------------------------------

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CatalogId = product.CatalogId
            };
        }

        // ---------------------------------
        // UPDATE PRODUCT INFO (DDD WAY)
        // ---------------------------------

        public async Task UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new Exception("Product not found");

            // ❗ DDD: логіка всередині entity
            product.UpdateInfo(
                dto.Name,
                dto.Description,
                dto.Price
            );

            await _productRepository.UpdateAsync(product);
        }

        // ---------------------------------
        // STOCK OPERATIONS (DDD WAY)
        // ---------------------------------

        public async Task IncreaseStockAsync(int id, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new Exception("Product not found");

            product.IncreaseStock(quantity);

            await _productRepository.UpdateAsync(product);
        }

        public async Task ReserveStockAsync(int id, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new Exception("Product not found");

            product.ReserveStock(quantity);

            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }
    }
}
