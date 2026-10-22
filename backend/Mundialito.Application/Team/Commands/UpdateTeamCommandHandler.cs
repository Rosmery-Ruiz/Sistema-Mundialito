using Mundialito.Application.Common.Interfaces;
using Mundialito.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Team.Commands
{
    public class UpdateTeamCommandHandler
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTeamCommandHandler(
            ITeamRepository teamRepository,
            IUnitOfWork unitOfWork)
        {
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            UpdateTeamCommand command,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                return Result.Failure(
                    "Team name is required.",
                    "VALIDATION_ERROR");
            }

            var team = await _teamRepository.GetByIdAsync(
                command.Id,
                cancellationToken);

            if (team is null)
            {
                return Result.Failure(
                    "Team not found.",
                    "NOT_FOUND");
            }

            team.Name = command.Name.Trim();

            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

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