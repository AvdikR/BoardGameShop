using BoardGameShop.Domain.Interfaces;

namespace BoardGameShop.Domain.Events
{
    public class OrderCreatedEvent : IDomainEvent
    {
        public int OrderId { get; }
        public int CustomerId { get; }

        public OrderCreatedEvent(int orderId, int customerId)
        {
            OrderId = orderId;
            CustomerId = customerId;
        }
    }
}
