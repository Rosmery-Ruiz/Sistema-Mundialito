using Mundialito.Application.Common.Interfaces;
using Mundialito.Application.Results;
using Mundialito.Application.Team.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Mundialito.Application.Team.Commands
{
    public class CreateTeamCommandHandler
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdempotencyRepository _idempotencyRepository;
        public CreateTeamCommandHandler(
     ITeamRepository teamRepository,
     IUnitOfWork unitOfWork,
     IIdempotencyRepository idempotencyRepository)
        {
            _teamRepository = teamRepository;
            _unitOfWork = unitOfWork;
            _idempotencyRepository = idempotencyRepository;
        }
    
        public async Task<Result<Domain.Entities.Team>> HandleAsync(
    CreateTeamCommand command,
    CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                return Result<Domain.Entities.Team>.Failure(
                    "Team name is required.",
                    "VALIDATION_ERROR");
            }
            var existingRequest = await _idempotencyRepository.GetByKeyAsync(
    command.IdempotencyKey,
    cancellationToken);

            if (existingRequest != null)
            {
                var existingTeam = JsonSerializer.Deserialize<Domain.Entities.Team>(
                    existingRequest.ResponseBody);

                return Result<Domain.Entities.Team>.Success(existingTeam!);
            }

            var team = new Domain.Entities.Team
            {
                Name = command.Name.Trim()
            };


            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                await _teamRepository.AddAsync(team, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var idempotencyRecord = new Domain.Entities.IdempotencyRecord
                {
                    Key = command.IdempotencyKey,
                    ResponseBody = JsonSerializer.Serialize(team),
                    StatusCode = 201,
                    CreatedAt = DateTime.UtcNow
                };

                await _idempotencyRepository.AddAsync(
                    idempotencyRecord,
                    cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);


                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }

            return Result<Domain.Entities.Team>.Success(team);
        }
    }
}