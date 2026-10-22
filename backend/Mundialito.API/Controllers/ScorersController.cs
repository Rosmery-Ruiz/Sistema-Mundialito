using Microsoft.AspNetCore.Mvc;
using Mundialito.Application.Scorers.Queries;
using Mundialito.Application.Scorers;

namespace Mundialito.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScorersController : ControllerBase
    {
        private readonly GetScorersQueryHandler _getScorersHandler;

        public ScorersController(
            GetScorersQueryHandler getScorersHandler)
        {
            _getScorersHandler = getScorersHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] GetScorersQuery query,
    CancellationToken cancellationToken)
        {
            var result = await _getScorersHandler.HandleAsync(
                query,
                cancellationToken);

            return Ok(result);
        }
    }
}