using System.Threading.Tasks;

namespace BoardGameShop.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();

        Task CommitAsync();

        Task RollbackAsync();

        Task SaveChangesAsync();
    }
}
