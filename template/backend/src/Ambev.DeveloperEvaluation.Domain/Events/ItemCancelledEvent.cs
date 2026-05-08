using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a sale item is cancelled.
/// </summary>
public class ItemCancelledEvent
{
    /// <summary>
    /// Gets the cancelled item.
    /// </summary>
    public SaleItem Item { get; }

    /// <summary>
    /// Gets the sale ID containing the item.
    /// </summary>
    public Guid SaleId { get; }

    /// <summary>
    /// Initializes a new instance of the ItemCancelledEvent class.
    /// </summary>
    /// <param name="item">The cancelled item</param>
    /// <param name="saleId">The sale ID</param>
    public ItemCancelledEvent(SaleItem item, Guid saleId)
    {
        Item = item;
        SaleId = saleId;
    }
}
