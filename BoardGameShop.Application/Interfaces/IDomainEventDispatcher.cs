using BoardGameShop.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoardGameShop.Application.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAndClearEvents(IEnumerable<BaseEntity> entities);
    }
}
