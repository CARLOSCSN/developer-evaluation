using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Handler for processing DeleteSaleCommand requests
/// </summary>
public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, DeleteSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<DeleteSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of DeleteSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="logger">The logger instance</param>
    public DeleteSaleHandler(
        ISaleRepository saleRepository,
        ILogger<DeleteSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the DeleteSaleCommand request
    /// </summary>
    /// <param name="command">The DeleteSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the operation</returns>
    public async Task<DeleteSaleResult> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        if (sale.Cancelled)
        {
            return new DeleteSaleResult
            {
                Success = false,
                Message = "Sale is already cancelled"
            };
        }

        // Perform logical deletion (cancellation)
        var deleted = await _saleRepository.DeleteAsync(command.Id, cancellationToken);

        if (deleted)
        {
            // Log domain event
            var saleCancelledEvent = new SaleCancelledEvent(sale);
            _logger.LogInformation(
                "Sale cancelled: ID={SaleId}, SaleNumber={SaleNumber}, Customer={CustomerName}, Branch={Branch}, TotalAmount={TotalAmount}",
                sale.Id,
                sale.SaleNumber,
                sale.CustomerName,
                sale.Branch,
                sale.TotalAmount);

            return new DeleteSaleResult
            {
                Success = true,
                Message = "Sale cancelled successfully"
            };
        }

        return new DeleteSaleResult
        {
            Success = false,
            Message = "Failed to cancel sale"
        };
    }
}
