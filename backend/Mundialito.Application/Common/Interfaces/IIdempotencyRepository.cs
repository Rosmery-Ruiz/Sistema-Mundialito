using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mundialito.Domain.Entities;

namespace Mundialito.Application.Common.Interfaces
{
    public interface IIdempotencyRepository
    {
        Task<IdempotencyRecord?> GetByKeyAsync(
            string key,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            IdempotencyRecord record,
            CancellationToken cancellationToken = default);
    }
}