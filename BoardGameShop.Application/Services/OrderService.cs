using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BoardGameShop.Application.DTOs;
using BoardGameShop.Application.Interfaces;
using BoardGameShop.Domain.Entities;
using BoardGameShop.Domain.Interfaces;
using System.Threading.Tasks;

namespace BoardGameShop.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly BoardGameShop.Domain.Interfaces.IUnitOfWork _unitOfWork;
        private readonly BoardGameShop.Domain.Interfaces.IPromotionRepository _promotionRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            BoardGameShop.Domain.Interfaces.IUnitOfWork unitOfWork,
            BoardGameShop.Domain.Interfaces.IPromotionRepository promotionRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _promotionRepository = promotionRepository;
        }

        public async Task<int> CreateAsync(CreateOrderDto dto)
        {
            var items = dto.Items.Select(i => (i.ProductId, i.Quantity)).ToList();

            return await CreateAsync(dto.CustomerId, items);
        }

        // ---------------------------------
        // CREATE ORDER
        // ---------------------------------

        public async Task<int> CreateAsync(int customerId, List<(int productId, int quantity)> items)
        {
            var order = new Order(customerId);

            // Attach customer if available for loyalty pricing
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer != null)
            {
                order.AssignCustomer(customer);
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                foreach (var item in items)
                {
                    var product = await _productRepository.GetByIdAsync(item.productId);

                    if (product == null)
                        throw new Exception($"Product {item.productId} not found");

                    // DDD: order aggregate enforces business rules and reserves stock
                    // Product is loaded and tracked by EF; changes (stock) will be detected by UnitOfWork/DbContext.
                    order.AddItem(product, item.quantity);
                }

                await _orderRepository.AddAsync(order);

                // persist changes
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            return order.Id;
        }

        // ---------------------------------
        // GET BY ID
        // ---------------------------------

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                return null;

            // fetch active promotions
            var promotions = await _promotionRepository.GetActivePromotionsAsync(DateTime.UtcNow);

            var domainPricing = Domain.Services.PricingCalculator.CalculateDetailed(order.OrderItems, order.Customer != null ? order.Customer.LoyaltyTier : Domain.Enums.LoyaltyTier.Bronze, promotions);

            var pricing = new PricingDto
            {
                Subtotal = domainPricing.Subtotal,
                BaseDiscount = domainPricing.BaseDiscount,
                LoyaltyDiscount = domainPricing.LoyaltyDiscount,
                PromotionsDiscount = domainPricing.PromotionsDiscount,
                AppliedPromotions = domainPricing.AppliedPromotions.Select(p => new AppliedPromotionDto { Name = p.Name, Percentage = p.Percentage, DiscountAmount = p.DiscountAmount }).ToList(),
                Total = domainPricing.Total
            };

                return new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                TotalPrice = pricing.Total,
                Status = order.Status,
                CustomerLoyalty = order.Customer != null ? order.Customer.LoyaltyTier.ToString() : string.Empty,
                    Pricing = pricing,
                    Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };
        }

        // ---------------------------------
        // CONFIRM ORDER (WORKFLOW)
        // ---------------------------------

        public async Task ConfirmAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
                throw new Exception("Order not found");

            // ❗ DDD: логіка всередині Aggregate
            order.Confirm();

            await _orderRepository.UpdateAsync(order);
        }

        // ---------------------------------
        // CANCEL ORDER (WORKFLOW)
        // ---------------------------------

        public async Task CancelAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
                throw new Exception("Order not found");

            // Restore stock and cancel within a unit of work transaction
            var updatedProducts = new List<Product>();

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                foreach (var item in order.OrderItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.IncreaseStock(item.Quantity);
                        updatedProducts.Add(product);
                        await _productRepository.UpdateAsync(product);
                    }
                }

                order.Cancel();

                await _orderRepository.UpdateAsync(order);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        // ---------------------------------
        // GET ALL
        // ---------------------------------

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            var promotions = await _promotionRepository.GetActivePromotionsAsync(DateTime.UtcNow);

            return orders.Select(order => {
                var domainPricing = Domain.Services.PricingCalculator.CalculateDetailed(order.OrderItems, order.Customer != null ? order.Customer.LoyaltyTier : Domain.Enums.LoyaltyTier.Bronze, promotions);

                var pricing = new PricingDto
                {
                    Subtotal = domainPricing.Subtotal,
                    BaseDiscount = domainPricing.BaseDiscount,
                    LoyaltyDiscount = domainPricing.LoyaltyDiscount,
                    PromotionsDiscount = domainPricing.PromotionsDiscount,
                    AppliedPromotions = domainPricing.AppliedPromotions.Select(p => new AppliedPromotionDto { Name = p.Name, Percentage = p.Percentage, DiscountAmount = p.DiscountAmount }).ToList(),
                    Total = domainPricing.Total
                };

                return new OrderDto
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    OrderDate = order.OrderDate,
                    TotalPrice = pricing.Total,
                    Status = order.Status,
                    CustomerLoyalty = order.Customer != null ? order.Customer.LoyaltyTier.ToString() : string.Empty,
                    Pricing = pricing
                };
            });
        }
    }
}
