using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a sale is modified.
/// </summary>
public class SaleModifiedEvent
{
    /// <summary>
    /// Gets the modified sale.
    /// </summary>
    public Sale Sale { get; }

    /// <summary>
    /// Initializes a new instance of the SaleModifiedEvent class.
    /// </summary>
    /// <param name="sale">The modified sale</param>
    public SaleModifiedEvent(Sale sale)
    {
        Sale = sale;
    }
}
