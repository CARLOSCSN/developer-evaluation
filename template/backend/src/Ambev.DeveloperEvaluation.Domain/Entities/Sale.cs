using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale in the system.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets or sets the sale number (unique identifier for the sale).
    /// Generated automatically.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the sale was made.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the customer ID (External Identity).
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the customer name (denormalized).
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch where the sale was made.
    /// </summary>
    public string Branch { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of sale items.
    /// </summary>
    public List<SaleItem> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the total amount of the sale (sum of all items after discount).
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets whether the sale is cancelled.
    /// </summary>
    public bool Cancelled { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the sale was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the last update to the sale.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale()
    {
        CreatedAt = DateTime.UtcNow;
        SaleNumber = Guid.NewGuid().ToString("N")[..16].ToUpper();
        Date = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates the total amount of the sale based on all items.
    /// </summary>
    public void CalculateTotalAmount()
    {
        TotalAmount = Items.Where(i => !i.Cancelled).Sum(item => item.TotalItemAmount);
    }

    /// <summary>
    /// Cancels the sale logically.
    /// </summary>
    public void Cancel()
    {
        Cancelled = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the sale timestamp.
    /// </summary>
    public void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
