using BoardGameShop.Application.Interfaces;
using BoardGameShop.Domain.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BoardGameShop.Infrastructure.Services
{
    public class SimpleEventDispatcher : IDomainEventDispatcher
    {
        private readonly ILogger<SimpleEventDispatcher> _logger;

        public SimpleEventDispatcher(ILogger<SimpleEventDispatcher> logger)
        {
            _logger = logger;
        }

        public Task DispatchAndClearEvents(IEnumerable<BaseEntity> entities)
        {
            foreach (var entity in entities)
            {
                var events = entity.DomainEvents.ToArray();
                entity.ClearDomainEvents();

                foreach (var domainEvent in events)
                {
                    _logger.LogInformation($"[Domain Event] Оброблено подію: {domainEvent.GetType().Name}");
                }
            }
            return Task.CompletedTask;
        }
    }
}
