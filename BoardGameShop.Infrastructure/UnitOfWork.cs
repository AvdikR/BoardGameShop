using BoardGameShop.Domain.Interfaces;
using BoardGameShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace BoardGameShop.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BoardGameShopDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(BoardGameShopDbContext context)
        {
            _context = context;
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
            await _context.SaveChangesAsync();
        }
    }
}
