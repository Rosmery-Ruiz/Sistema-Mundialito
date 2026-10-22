using Mundialito.Application.Common.Interfaces;
using Mundialito.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Match.Commands
{
    public class CreateMatchCommandHandler
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMatchCommandHandler(
            IMatchRepository matchRepository,
            ITeamRepository teamRepository,
            IUnitOfWork unitOfWork)
        {
            _matchRepository = matchRepository;
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Mundialito.Domain.Entities.Match>> HandleAsync(
            CreateMatchCommand command,
            CancellationToken cancellationToken = default)
        {
            if (command.HomeTeamId == command.AwayTeamId)
            {
                return Result<Mundialito.Domain.Entities.Match>.Failure(
                    "Home team and away team must be different.",
                    "VALIDATION_ERROR");
            }

            var homeTeam = await _teamRepository.GetByIdAsync(
                command.HomeTeamId,
                cancellationToken);

            if (homeTeam is null)
            {
                return Result<Mundialito.Domain.Entities.Match>.Failure(
                    "Home team not found.",
                    "NOT_FOUND");
            }

            var awayTeam = await _teamRepository.GetByIdAsync(
                command.AwayTeamId,
                cancellationToken);

            if (awayTeam is null)
            {
                return Result<Mundialito.Domain.Entities.Match>.Failure(
                    "Away team not found.",
                    "NOT_FOUND");
            }

            var match = new Mundialito.Domain.Entities.Match
            {
                HomeTeamId = command.HomeTeamId,
                AwayTeamId = command.AwayTeamId,
                MatchDate = command.MatchDate,
                HomeScore = null,
                AwayScore = null,
                Status = "Scheduled"
            };

            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                await _matchRepository.AddAsync(
                    match,
                    cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }

            return Result<Mundialito.Domain.Entities.Match>.Success(match);
        }
    }
}