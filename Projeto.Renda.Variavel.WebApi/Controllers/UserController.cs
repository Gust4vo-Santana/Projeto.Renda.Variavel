using Application.UseCases.User.GetTotalInvestedByAssetUseCase;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Projeto.Renda.Variavel.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{apiVersion:apiVersion}/user")]
    public class UserController : ControllerBase
    {
        private readonly IGetTotalInvestedByAssetUseCase _getTotalInvestedByAssetUseCase;
        private readonly ILogger<UserController> _logger;

        public UserController(IGetTotalInvestedByAssetUseCase getTotalInvestedByAssetUseCase,
                              ILogger<UserController> logger)
        {
            _getTotalInvestedByAssetUseCase = getTotalInvestedByAssetUseCase;
            _logger = logger;
        }

        [HttpGet("total-by-asset")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTotalInvestedByAsset([FromQuery] long userId, [FromQuery] long assetId, CancellationToken cancellationToken)
        {
            var output = await _getTotalInvestedByAssetUseCase.ExecuteAsync(userId, assetId, cancellationToken);

            if (!output.IsValid)
                return BadRequest(output.GetResult());

            return Ok(output);
        }
    }
}
