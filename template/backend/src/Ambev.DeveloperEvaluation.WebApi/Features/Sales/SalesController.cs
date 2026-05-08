using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller for managing sale operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of SalesController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new sale
    /// </summary>
    /// <param name="request">The sale creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new object(), _mapper.Map<CreateSaleResponse>(response));
    }

    /// <summary>
    /// Retrieves all sales with optional filtering and pagination
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="size">Page size (default: 10)</param>
    /// <param name="order">Optional order clause (e.g., "date desc, saleNumber asc")</param>
    /// <param name="branch">Optional branch filter</param>
    /// <param name="customerId">Optional customer ID filter</param>
    /// <param name="minDate">Optional minimum date filter</param>
    /// <param name="maxDate">Optional maximum date filter</param>
    /// <param name="cancelled">Optional cancelled status filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of sales</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSalesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSales(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_size")] int size = 10,
        [FromQuery(Name = "_order")] string? order = null,
        [FromQuery] string? branch = null,
        [FromQuery] int? customerId = null,
        [FromQuery(Name = "_minDate")] DateTime? minDate = null,
        [FromQuery(Name = "_maxDate")] DateTime? maxDate = null,
        [FromQuery] bool? cancelled = null,
        CancellationToken cancellationToken = default)
    {
        var command = new GetSalesCommand
        {
            Page = page,
            Size = size,
            Order = order,
            Branch = branch,
            CustomerId = customerId,
            MinDate = minDate,
            MaxDate = maxDate,
            Cancelled = cancelled
        };

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<GetSalesResponse>(response));
    }

    /// <summary>
    /// Retrieves a sale by its ID
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale details if found</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new GetSaleCommand(id);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<GetSaleResponse>(response));
    }

    /// <summary>
    /// Updates an existing sale
    /// </summary>
    /// <param name="id">The unique identifier of the sale to update</param>
    /// <param name="request">The sale update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSale([FromRoute] Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateSaleCommand>(request);
        command.Id = id;
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<UpdateSaleResponse>(response));
    }

    /// <summary>
    /// Deletes (cancels) a sale by its ID
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the sale was deleted</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteSaleCommand(id);
        await _mediator.Send(command, cancellationToken);

        return Ok(new { }); // BaseController.Ok will wrap this
    }

    /// <summary>
    /// Cancels a specific item in a sale
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="itemId">The unique identifier of the item to cancel</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response with updated total amount</returns>
    [HttpPatch("{id}/items/{itemId}")]
    [ProducesResponseType(typeof(ApiResponseWithData<CancelItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelItem([FromRoute] Guid id, [FromRoute] Guid itemId, CancellationToken cancellationToken)
    {
        var command = new CancelItemCommand(id, itemId);
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(new CancelItemResponse
        {
            Success = result.Success,
            Message = result.Message,
            UpdatedTotalAmount = result.UpdatedTotalAmount
        });
    }
}
