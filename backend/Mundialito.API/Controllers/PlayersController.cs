using Microsoft.AspNetCore.Mvc;
using Mundialito.Application.Player.Commands;
using Mundialito.Application.Player.Queries;

namespace Mundialito.Api.Controllers
{
    [ApiController]
    [Route("api/players")]
    public class PlayersController : ControllerBase
    {
        private readonly CreatePlayerCommandHandler _createPlayerHandler;
        private readonly GetPlayersQueryHandler _getPlayersHandler;

        public PlayersController(
            CreatePlayerCommandHandler createPlayerHandler,
            GetPlayersQueryHandler getPlayersHandler)
        {
            _createPlayerHandler = createPlayerHandler;
            _getPlayersHandler = getPlayersHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreatePlayerCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _createPlayerHandler.HandleAsync(
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

        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] GetPlayersQuery query,
    CancellationToken cancellationToken)
        {
            var result = await _getPlayersHandler.HandleAsync(
                query,
                cancellationToken);

            return Ok(result);
        }
    }
}