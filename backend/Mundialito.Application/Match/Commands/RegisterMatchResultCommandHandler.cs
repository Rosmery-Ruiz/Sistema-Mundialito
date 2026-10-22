using Mundialito.Application.Common.Interfaces;
using Mundialito.Application.Results;
using Mundialito.Domain.Entities;

namespace Mundialito.Application.Match.Commands
{
    public class RegisterMatchResultCommandHandler
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGoalRepository _goalRepository;

        public RegisterMatchResultCommandHandler(
      IMatchRepository matchRepository,
      IGoalRepository goalRepository,
      IUnitOfWork unitOfWork)
        {
            _matchRepository = matchRepository;
            _goalRepository = goalRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            RegisterMatchResultCommand command,
            CancellationToken cancellationToken = default)
        {
            if (command.HomeScore < 0 || command.AwayScore < 0)
            {
                return Result.Failure(
                    "Scores cannot be negative.",
                    "VALIDATION_ERROR");
            }

            if (command.HomeScorerIds.Count != command.HomeScore)
            {
                return Result.Failure(
                    "The number of home scorers must match the home score.",
                    "VALIDATION_ERROR");
            }

            if (command.AwayScorerIds.Count != command.AwayScore)
            {
                return Result.Failure(
                    "The number of away scorers must match the away score.",
                    "VALIDATION_ERROR");
            }

            var match = await _matchRepository.GetByIdAsync(
                command.MatchId,
                cancellationToken);

            if (match is null)
            {
                return Result.Failure(
                    "Match not found.",
                    "NOT_FOUND");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                match.HomeScore = command.HomeScore;
                match.AwayScore = command.AwayScore;
                match.Status = "Finished";

                foreach (var playerId in command.HomeScorerIds)
                {
                    var goal = new Goal
                    {
                        MatchId = match.Id,
                        PlayerId = playerId
                    };

                    await _goalRepository.AddAsync(goal, cancellationToken);
                }

                foreach (var playerId in command.AwayScorerIds)
                {
                    var goal = new Goal
                    {
                        MatchId = match.Id,
                        PlayerId = playerId
                    };

                    await _goalRepository.AddAsync(goal, cancellationToken);
                }

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