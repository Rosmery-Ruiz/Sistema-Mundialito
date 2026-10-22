using Microsoft.AspNetCore.Mvc;
using Mundialito.Application.Team.Command;
using Mundialito.Application.Team.Commands;
using Mundialito.Application.Team.Queries;

namespace Mundialito.Api.Controllers
{
    [ApiController]
    [Route("api/teams")]
    public class TeamsController : ControllerBase
    {
        private readonly CreateTeamCommandHandler _createTeamHandler;
        private readonly GetTeamsQueryHandler _getTeamsHandler;
        private readonly UpdateTeamCommandHandler _updateTeamHandler;
        private readonly DeleteTeamCommandHandler _deleteTeamHandler;
        public TeamsController(
     CreateTeamCommandHandler createTeamHandler,
     GetTeamsQueryHandler getTeamsHandler,
     UpdateTeamCommandHandler updateTeamHandler,
     DeleteTeamCommandHandler deleteTeamHandler)
        {
            _createTeamHandler = createTeamHandler;
            _getTeamsHandler = getTeamsHandler;
            _updateTeamHandler = updateTeamHandler;
            _deleteTeamHandler = deleteTeamHandler;
        }


        [HttpPost]
        public async Task<IActionResult> Create(
    CreateTeamCommand command,
    [FromHeader(Name = "Idempotency-Key")] string idempotencyKey,
    CancellationToken cancellationToken)
        {
            command.IdempotencyKey = idempotencyKey;


            var result = await _createTeamHandler.HandleAsync(command,cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    result.ErrorCode,
                    result.ErrorMessage
                });
            }
            return StatusCode(201, result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] GetTeamsQuery query,
    CancellationToken cancellationToken)
        {
            var result = await _getTeamsHandler.HandleAsync(
                query,
                cancellationToken);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
    int id,
    UpdateTeamCommand command,
    CancellationToken cancellationToken)
        {
            command.Id = id;

            var result = await _updateTeamHandler.HandleAsync(
                command,
                cancellationToken);

            if (!result.IsSuccess)
            {
                if (result.ErrorCode == "NOT_FOUND")
                {
                    return NotFound(new
                    {
                        result.ErrorCode,
                        result.ErrorMessage
                    });
                }

                return BadRequest(new
                {
                    result.ErrorCode,
                    result.ErrorMessage
                });
            }

            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
    int id,
    CancellationToken cancellationToken)
        {
            var command = new DeleteTeamCommand
            {
                Id = id
            };

            var result = await _deleteTeamHandler.HandleAsync(
                command,
                cancellationToken);

            if (!result.IsSuccess)
            {
                if (result.ErrorCode == "NOT_FOUND")
                {
                    return NotFound(new
                    {
                        result.ErrorCode,
                        result.ErrorMessage
                    });
                }

                return BadRequest(new
                {
                    result.ErrorCode,
                    result.ErrorMessage
                });
            }

            return NoContent();
        }
    }
}