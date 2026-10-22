using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync(
      CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default);
        Task CommitAsync(
        CancellationToken cancellationToken = default);
        Task RollbackAsync(
            CancellationToken cancellationToken = default);
    }
}
