using BoardGameShop.Application.Interfaces;
using BoardGameShop.Domain.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace BoardGameShop.Application.Services
{
    public class SimpleEventDispatcher : IDomainEventDispatcher
    {
        public Task DispatchAndClearEvents(IEnumerable<BaseEntity> entities)
        {
            foreach (var entity in entities)
            {
                var events = entity.DomainEvents.ToArray();
                entity.ClearDomainEvents();

                foreach (var domainEvent in events)
                {
                    System.Console.WriteLine($"[Domain Event] Оброблено подію: {domainEvent.GetType().Name}");
                }
            }
            return Task.CompletedTask;
        }
    }
}
