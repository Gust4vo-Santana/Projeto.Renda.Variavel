using Application.UseCases.Operation.GetBrokerageFeeByUser;
using Application.UseCases.Operation.GetGlobalAveragePriceByAsset;
using Application.UseCases.Operation.GetTopBrokerageFeePayers;
using Application.UseCases.Operation.GetTotalBrokerageFee;
using Microsoft.AspNetCore.Mvc;
using Projeto.Renda.Variavel.WebApi.Controllers.ApiResponse;
using Projeto.Renda.Variavel.WebApi.Dtos;
using Projeto.Renda.Variavel.WebApi.Mappers;
using System.Net.Mime;

namespace Projeto.Renda.Variavel.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{apiVersion:apiVersion=1.0}/operation")]
    public class OperationController : ControllerBase
    {
        private readonly IGetBrokerageFeeByUserUseCase _getBrokerageFeeByUserUseCase;
        private readonly IGetTotalBrokerageFeeUseCase _getTotalBrokerageFeeUseCase;
        private readonly IGetGlobalAveragePriceByAssetUseCase _getGlobalAveragePriceByAssetUseCase;
        private readonly IGetTopBrokerageFeePayersUseCase _getTopBrokerageFeePayersUseCase;
        private readonly ILogger<OperationController> _logger;

        public OperationController(IGetBrokerageFeeByUserUseCase getBrokerageFeeByUserUseCase,
                                   IGetTotalBrokerageFeeUseCase getTotalBrokerageFeeUseCase,
                                   IGetGlobalAveragePriceByAssetUseCase getGlobalAveragePriceByAssetUseCase,
                                   IGetTopBrokerageFeePayersUseCase getTopBrokerageFeePayersUseCase,
                                   ILogger<OperationController> logger)
        {
            _getBrokerageFeeByUserUseCase = getBrokerageFeeByUserUseCase;
            _getTotalBrokerageFeeUseCase = getTotalBrokerageFeeUseCase;
            _getGlobalAveragePriceByAssetUseCase = getGlobalAveragePriceByAssetUseCase;
            _getTopBrokerageFeePayersUseCase = getTopBrokerageFeePayersUseCase;
            _logger = logger;
        }

        /// <summary>
        /// Retorna o total de corretagem pago pelo usuario especificado pelo ID
        /// </summary>
        [HttpGet("brokerage-fee-by-user")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<BrokerageFeeDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<BrokerageFeeDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBrokerageFeeByUserAsync([FromQuery] GetBrokerageFeeByUserInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetBrokerageFeeByUser called with userId: {UserId}", input.UserId);

            var output = await _getBrokerageFeeByUserUseCase.ExecuteAsync(input, cancellationToken);

            if (!output.IsValid)
            {
                return BadRequest(new ApiResponse<BrokerageFeeDto>()
                {
                    Errors = output.GetErrorMessages()
                });
            }

            return Ok(new ApiResponse<BrokerageFeeDto>()
            {
                Data = output.GetResult()!.MapToBrokerageFeeDto()
            });
        }

        /// <summary>
        /// Retorna o valor financeiro ganho pela corretora com as corretagens
        /// </summary>
        [HttpGet("total-brokerage-fee")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<BrokerageFeeDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<BrokerageFeeDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTotalBrokerageFee(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetTotalBrokerageFee called");

            var output = await _getTotalBrokerageFeeUseCase.ExecuteAsync(cancellationToken);

            if (!output.IsValid)
            {
                return BadRequest(new ApiResponse<BrokerageFeeDto>()
                {
                    Errors = output.GetErrorMessages()
                });
            }

            return Ok(new ApiResponse<BrokerageFeeDto>()
            {
                Data = output.GetResult()!.MapToBrokerageFeeDto()
            });
        }

        /// <summary>
        ///  Retorna o preco medio ponderado de aquisicao de um ativo
        /// </summary>
        [HttpGet("global-average-price")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<AveragePriceDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<AveragePriceDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGlobalAveragePriceByAssetAsync([FromQuery] GetGlobalAveragePriceByAssetInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetGlobalAveragePriceByAsset called with assetId: {AssetId}", input.AssetId);

            var output = await _getGlobalAveragePriceByAssetUseCase.ExecuteAsync(input, cancellationToken);

            if (!output.IsValid)
            {
                return BadRequest(new ApiResponse<AveragePriceDto>()
                {
                    Errors = output.GetErrorMessages()
                });
            }

            return Ok(new ApiResponse<AveragePriceDto>()
            {
                Data = output.GetResult().MapToAveragePriceDto()
            });
        }

        /// <summary>
        /// Retorna os top 10 usuarios que pagaram mais corretagem
        /// </summary>
        [HttpGet("top-brokerage-fee-payers")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<UserIdDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<IEnumerable<UserIdDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTopBrokerageFeePayersAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetTopBrokerageFeePayers was called");

            var output = await _getTopBrokerageFeePayersUseCase.ExecuteAsync(cancellationToken);

            if (!output.IsValid)
            {
                return BadRequest(new ApiResponse<IEnumerable<UserIdDto>>()
                {
                    Errors = output.GetErrorMessages()
                });
            }

            return Ok(new ApiResponse<IEnumerable<UserIdDto>>()
            {
                Data = output.GetResult().MapToDtoList()
            });
        }
    }
}
