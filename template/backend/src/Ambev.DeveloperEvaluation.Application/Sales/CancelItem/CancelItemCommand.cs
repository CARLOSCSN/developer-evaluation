using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

/// <summary>
/// Command for cancelling a specific item in a sale
/// </summary>
public class CancelItemCommand : IRequest<CancelItemResult>
{
    /// <summary>
    /// Gets or sets the sale ID
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the item ID to cancel
    /// </summary>
    public Guid ItemId { get; set; }

    /// <summary>
    /// Initializes a new instance of CancelItemCommand
    /// </summary>
    /// <param name="saleId">The sale ID</param>
    /// <param name="itemId">The item ID</param>
    public CancelItemCommand(Guid saleId, Guid itemId)
    {
        SaleId = saleId;
        ItemId = itemId;
    }
}
