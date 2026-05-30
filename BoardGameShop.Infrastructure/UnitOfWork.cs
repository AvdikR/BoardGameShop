using BoardGameShop.Application.Interfaces;
using BoardGameShop.Domain.Common;
using BoardGameShop.Domain.Interfaces;
using BoardGameShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameShop.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BoardGameShopDbContext _context;
        private readonly IDomainEventDispatcher _dispatcher;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(BoardGameShopDbContext context, IDomainEventDispatcher dispatcher)
        {
            _context = context;
            _dispatcher = dispatcher;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task SaveChangesAsync()
        {
            // Зберігаємо зміни в БД
            await _context.SaveChangesAsync();

            // Шукаємо всі сутності, які згенерували події
            var entitiesWithEvents = _context.ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToList();

            // Відправляємо події
            if (entitiesWithEvents.Any())
            {
                await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);
            }
        }
    }
}
