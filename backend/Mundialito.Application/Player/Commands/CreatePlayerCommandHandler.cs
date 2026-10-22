using Mundialito.Application.Common.Interfaces;
using Mundialito.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Player.Commands
{
    public class CreatePlayerCommandHandler
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePlayerCommandHandler(
            IPlayerRepository playerRepository,
            ITeamRepository teamRepository,
            IUnitOfWork unitOfWork)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Mundialito.Domain.Entities.Player>> HandleAsync(
            CreatePlayerCommand command,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                return Result<Mundialito.Domain.Entities.Player>.Failure(
                    "Player name is required.",
                    "VALIDATION_ERROR");
            }

            var team = await _teamRepository.GetByIdAsync(
                command.TeamId,
                cancellationToken);

            if (team is null)
            {
                return Result<Mundialito.Domain.Entities.Player>.Failure(
                    "Team not found.",
                    "NOT_FOUND");
            }

            var player = new Mundialito.Domain.Entities.Player
            {
                TeamId = command.TeamId,
                Name = command.Name.Trim(),
                ShirtNumber = command.ShirtNumber
            };

            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                await _playerRepository.AddAsync(
                    player,
                    cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }

            return Result<Mundialito.Domain.Entities.Player>.Success(player);
        }
    }
}