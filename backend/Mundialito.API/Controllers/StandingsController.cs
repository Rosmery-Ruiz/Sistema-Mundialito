using Microsoft.AspNetCore.Mvc;
using Mundialito.Application.Standings.Queries;

namespace Mundialito.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StandingsController : ControllerBase
    {
        private readonly GetStandingsQueryHandler _getStandingsHandler;

        public StandingsController(
            GetStandingsQueryHandler getStandingsHandler)
        {
            _getStandingsHandler = getStandingsHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
     [FromQuery] GetStandingsQuery query,
     CancellationToken cancellationToken)
        {
            var result = await _getStandingsHandler.HandleAsync(
                query,
                cancellationToken);

            return Ok(result);
        }
    }
    }
