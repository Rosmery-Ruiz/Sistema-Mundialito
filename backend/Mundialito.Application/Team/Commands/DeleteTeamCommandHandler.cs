using Mundialito.Application.Common.Interfaces;
using Mundialito.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Team.Commands
{
    public class DeleteTeamCommandHandler
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTeamCommandHandler(
            ITeamRepository teamRepository,
            IUnitOfWork unitOfWork)
        {
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            DeleteTeamCommand command,
            CancellationToken cancellationToken = default)
        {
            var team = await _teamRepository.GetByIdAsync(
                command.Id,
                cancellationToken);

            if (team is null)
            {
                return Result.Failure(
                    "Team not found.",
                    "NOT_FOUND");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                _teamRepository.Delete(team);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }

            return Result.Success();
        }
    }
}