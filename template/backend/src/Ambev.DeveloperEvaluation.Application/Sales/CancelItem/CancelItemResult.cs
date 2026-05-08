namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

/// <summary>
/// Result for CancelItem operation
/// </summary>
public class CancelItemResult
{
    /// <summary>
    /// Gets or sets whether the operation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets a message about the operation
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated total amount of the sale
    /// </summary>
    public decimal UpdatedTotalAmount { get; set; }
}
