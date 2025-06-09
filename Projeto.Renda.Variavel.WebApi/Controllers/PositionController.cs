using Application.UseCases.Position.GetAveragePriceByUser;
using Application.UseCases.Position.GetGlobalPosition;
using Application.UseCases.Position.GetPositionByAsset;
using Application.UseCases.Position.GetTopUsersWithHighestPositions;
using Application.UseCases.Position.GetTotalInvestedByAsset;
using Microsoft.AspNetCore.Mvc;
using Projeto.Renda.Variavel.WebApi.Controllers.ApiResponse;
using Projeto.Renda.Variavel.WebApi.Dtos;
using Projeto.Renda.Variavel.WebApi.Mappers;
using System.Net.Mime;

namespace Projeto.Renda.Variavel.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{apiVersion:apiVersion=1.0}/position")]
    public class PositionController : ControllerBase
    {
        private readonly IGetTotalInvestedByAssetUseCase _getTotalInvestedByAssetUseCase;
        private readonly IGetGlobalPositionUseCase _getGlobalPositionUseCase;
        private readonly IGetAveragePriceByUserUseCase _getAveragePriceByUserUseCase;
        private readonly IGetPositionByAssetUseCase _getPositionByAssetUseCase;
        private readonly IGetTopUsersWithHighestPositionsUseCase _getTopUsersWithHighestPositionsUseCase;
        private readonly ILogger<PositionController> _logger;

        public PositionController(IGetTotalInvestedByAssetUseCase getTotalInvestedByAssetUseCase,
                              IGetGlobalPositionUseCase getGlobalPositionUseCase,
                              IGetAveragePriceByUserUseCase getAveragePriceByUserUseCase,
                              IGetPositionByAssetUseCase getPositionByAssetUseCase,
                              IGetTopUsersWithHighestPositionsUseCase getTopUsersWithHighestPositionsUseCase,
                              ILogger<PositionController> logger)
        {
            _getTotalInvestedByAssetUseCase = getTotalInvestedByAssetUseCase;
            _getGlobalPositionUseCase = getGlobalPositionUseCase;
            _getAveragePriceByUserUseCase = getAveragePriceByUserUseCase;
            _getPositionByAssetUseCase = getPositionByAssetUseCase;
            _getTopUsersWithHighestPositionsUseCase = getTopUsersWithHighestPositionsUseCase;
            _logger = logger;
        }

        /// <summary>
        /// Retorna o total investido por um usuario em um ativo especifico
        /// </summary>
        [HttpGet("total-by-asset")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TotalInvestedDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<TotalInvestedDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTotalInvestedByAssetAsync([FromQuery] GetTotalInvestedByAssetInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetTotalInvestedByAsset called with userId: {UserId}, assetId: {AssetId}", input.UserId, input.AssetId);
            var output = await _getTotalInvestedByAssetUseCase.ExecuteAsync(input, cancellationToken);

            if (!output.IsValid)
            {
                return BadRequest(new ApiResponse<TotalInvestedDto>()
                { 
                    Errors = output.GetErrorMessages()
                });
            }

            return Ok(new ApiResponse<TotalInvestedDto>()
            {
                Data = output.GetResult().MapToTotalInvestedDto()
            });
        }

        /// <summary>
        /// Retorna a posicao global do usuario especificado pelo ID
        /// </summary>
        [HttpGet("global-position")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<PositionDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<IEnumerable<PositionDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGlobalPositionAsync([FromQuery] GetGlobalPositionInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetGlobalPosition called with userId: {UserId}", input.UserId);

            var output = await _getGlobalPositionUseCase.ExecuteAsync(input, cancellationToken);

            if (!output.IsValid)
            {
                return BadRequest(new ApiResponse<IEnumerable<PositionDto>>()
                {
                    Errors = output.GetErrorMessages(),
                });
            }

            return Ok(new ApiResponse<IEnumerable<PositionDto>>()
            {
                Data = output.GetResult().MapToDtoList()
            });
        }

        /// <summary>
        /// Retorna o preco medio de um ativo pago pelo usuario especificado pelo ID
        /// </summary>
        [HttpGet("average-price")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<AveragePriceDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<AveragePriceDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAveragePriceAsync([FromQuery] GetAveragePriceByUserInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetAveragePrice called with userId: {UserId} and assetId: {AssetId}", input.UserId, input.AssetId);

            var output = await _getAveragePriceByUserUseCase.ExecuteAsync(input, cancellationToken);

            if (!output.IsValid)
            {
                return BadRequest(new ApiResponse<AveragePriceDto>()
                {
                    Errors = output.GetErrorMessages(),
                });
            }

            return Ok(new ApiResponse<AveragePriceDto>()
            {
                Data = output.GetResult().MapToAveragePriceDto()
            });
        }

        /// <summary>
        /// Retorna a posicao do cliente para o ativo especificado
        /// </summary>
        [HttpGet("position-by-asset")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<PositionDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<PositionDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPositionByAssetAsync([FromQuery] GetPositionByAssetInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetPositionByAssetController called with userId: {UserId} and assetId: {AssetId}", input.UserId, input.AssetId);

            var output = await _getPositionByAssetUseCase.ExecuteAsync(input, cancellationToken);

            if (!output.IsValid)
            {
                return BadRequest(new ApiResponse<PositionDto>()
                {
                    Errors = output.GetErrorMessages(),
                });
            }

            return Ok(new ApiResponse<PositionDto>()
            {
                Data = output.GetResult()!.MapToDto()
            });
        }

        /// <summary>
        /// Retorna os top 10 usuarios com maiores posicoes, levando em conta o valor total investido em todas as posicoes de cada cliente
        /// </summary>
        [HttpGet("top-users")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<UserIdDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<IEnumerable<UserIdDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTopUsersWithHighestPositionsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetTopUsersWithHighestPositions called");

            var output = await _getTopUsersWithHighestPositionsUseCase.ExecuteAsync(cancellationToken);

            if (!output.IsValid)
            {
                return BadRequest(new ApiResponse<IEnumerable<UserIdDto>>()
                {
                    Errors = output.GetErrorMessages(),
                });
            }

            return Ok(new ApiResponse<IEnumerable<UserIdDto>>()
            {
                Data = output.GetResult().MapToDtoList()
            });
        }
    }
}
