using System;
using System.Collections.Generic;
using System.Text;
using BoardGameShop.Application.DTOs;
using BoardGameShop.Application.Interfaces;
using BoardGameShop.Domain.Entities;
using BoardGameShop.Domain.Interfaces;

namespace BoardGameShop.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Status = o.Status.ToString(),

                Items = o.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            });
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var o = await _orderRepository.GetByIdAsync(id);

            if (o == null)
                return null;

            return new OrderDto
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Status = o.Status.ToString(),

                Items = o.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };
        }

        public async Task CreateAsync(CreateOrderDto dto)
        {
            var order = new Order
            {
                CustomerId = dto.CustomerId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Created,
                OrderItems = new List<OrderItem>()
            };

            decimal total = 0;

            foreach (var item in dto.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });

                total += item.Price * item.Quantity;
            }

            order.TotalPrice = total;

            await _orderRepository.AddAsync(order);
        }
    }
}
