using Application.UseCases.Quote.GetLatestQuote;
using Microsoft.AspNetCore.Mvc;
using Projeto.Renda.Variavel.WebApi.Controllers.ApiResponse;
using Projeto.Renda.Variavel.WebApi.Dtos;
using Projeto.Renda.Variavel.WebApi.Mappers;
using System.Net.Mime;

namespace Projeto.Renda.Variavel.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{apiVersion:apiVersion}/quote")]
    public class QuoteController : ControllerBase
    {
        private readonly IGetLatestQuoteUseCase _getLatestQuoteUseCase;
        private readonly ILogger<QuoteController> _logger;

        public QuoteController(IGetLatestQuoteUseCase getLatestQuoteUseCase, ILogger<QuoteController> logger)
        {
            _getLatestQuoteUseCase = getLatestQuoteUseCase;
            _logger = logger;
        }

        /// <summary>
        /// Retorna a última cotação de um ativo especificado pelo ID
        /// </summary>
        [HttpGet("latest-quote")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<QuoteDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<QuoteDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLatestQuoteAsync([FromQuery] GetLatestQuoteInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetLatestQuote called with assetId: {AssetId}", input.AssetId);
            
            var output = await _getLatestQuoteUseCase.ExecuteAsync(input, cancellationToken);

            if (!output.IsValid)
            {
                return BadRequest(new ApiResponse<QuoteDto>()
                {
                    Errors = output.GetErrorMessages()
                });
            }

            return Ok(new ApiResponse<QuoteDto>()
            {
                Data = output.GetResult()!.MapToDto()
            });
        }
    }
}
