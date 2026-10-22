using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mundialito.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Mundialito.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly MundialitoDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(MundialitoDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync(
         CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database /* Usa nuestro DbContext para pedirle a la base de datos que inicie una transacción*/
                .BeginTransactionAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(
           CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task CommitAsync(
           CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
            {
                await _transaction.CommitAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync(
          CancellationToken cancellationToken = default)
            {
                if (_transaction is not null)
                {
                await _transaction.RollbackAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
            }
        }
    }


