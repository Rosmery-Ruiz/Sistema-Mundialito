using Microsoft.AspNetCore.Mvc;
using Mundialito.Application.Match.Commands;
using Mundialito.Application.Match.Queries;

namespace Mundialito.Api.Controllers
{
    [ApiController]
    [Route("api/matches")]
    public class MatchesController : ControllerBase
    {
        private readonly CreateMatchCommandHandler _createMatchHandler;
        private readonly RegisterMatchResultCommandHandler _registerResultHandler;
        private readonly GetMatchesQueryHandler _getMatchesHandler;

        public MatchesController(
      CreateMatchCommandHandler createMatchHandler,
      RegisterMatchResultCommandHandler registerResultHandler,
      GetMatchesQueryHandler getMatchesHandler)
        {
            _createMatchHandler = createMatchHandler;
            _registerResultHandler = registerResultHandler;
            _getMatchesHandler = getMatchesHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateMatchCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _createMatchHandler.HandleAsync(
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

            return StatusCode(201, result.Value);
        }

        [HttpPut("{id}/result")]
        public async Task<IActionResult> RegisterResult(
    int id,
    RegisterMatchResultCommand command,
    CancellationToken cancellationToken)
        {
            command.MatchId = id;

            var result = await _registerResultHandler.HandleAsync(
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

        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] GetMatchesQuery query,
    CancellationToken cancellationToken)
        {
            var result = await _getMatchesHandler.HandleAsync(
                query,
                cancellationToken);

            return Ok(result);
        }
    }
}