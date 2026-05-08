using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

/// <summary>
/// Handler for processing CancelItemCommand requests
/// </summary>
public class CancelItemHandler : IRequestHandler<CancelItemCommand, CancelItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CancelItemHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CancelItemHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="logger">The logger instance</param>
    public CancelItemHandler(
        ISaleRepository saleRepository,
        ILogger<CancelItemHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the CancelItemCommand request
    /// </summary>
    /// <param name="command">The CancelItem command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the operation</returns>
    public async Task<CancelItemResult> Handle(CancelItemCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.SaleId} not found");

        if (sale.Cancelled)
            throw new InvalidOperationException("Cannot cancel an item from a cancelled sale");

        var item = sale.Items.FirstOrDefault(i => i.Id == command.ItemId);
        if (item == null)
            throw new KeyNotFoundException($"Item with ID {command.ItemId} not found in sale");

        if (item.Cancelled)
        {
            return new CancelItemResult
            {
                Success = false,
                Message = "Item is already cancelled",
                UpdatedTotalAmount = sale.TotalAmount
            };
        }

        // Cancel the item
        item.Cancel();

        // Recalculate sale total (excluding cancelled items)
        sale.CalculateTotalAmount();
        sale.MarkAsUpdated();

        // Save to database
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Log domain event
        var itemCancelledEvent = new ItemCancelledEvent(item, sale.Id);
        _logger.LogInformation(
            "Item cancelled: ItemID={ItemId}, SaleID={SaleId}, Product={ProductName}, Quantity={Quantity}",
            item.Id,
            sale.Id,
            item.ProductName,
            item.Quantity);

        return new CancelItemResult
        {
            Success = true,
            Message = "Item cancelled successfully",
            UpdatedTotalAmount = sale.TotalAmount
        };
    }
}
