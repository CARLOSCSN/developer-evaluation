using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a new sale is created.
/// </summary>
public class SaleCreatedEvent
{
    /// <summary>
    /// Gets the created sale.
    /// </summary>
    public Sale Sale { get; }

    /// <summary>
    /// Initializes a new instance of the SaleCreatedEvent class.
    /// </summary>
    /// <param name="sale">The created sale</param>
    public SaleCreatedEvent(Sale sale)
    {
        Sale = sale;
    }
}
