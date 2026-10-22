using Mundialito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Common.Interfaces
{
    public interface IGoalRepository
    {
        Task AddAsync(
            Goal goal,
            CancellationToken cancellationToken = default);
    }
}