using BoardGameShop.Application.DTOs;
using BoardGameShop.Domain.Entities;
using BoardGameShop.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace BoardGameShop.Application.UseCases.Orders
{
    public class CreateOrderCommandHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateOrderDto command)
        {
            var order = new Order(command.CustomerId);

            var customer = await _customerRepository.GetByIdAsync(command.CustomerId);
            if (customer != null) order.AssignCustomer(customer);

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                foreach (var item in command.Items)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product == null) throw new Exception($"Product {item.ProductId} not found");

                    order.AddItem(product, item.Quantity);
                    await _productRepository.UpdateAsync(product);
                }

                await _orderRepository.AddAsync(order);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return order.Id;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
