namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Result for DeleteSale operation
/// </summary>
public class DeleteSaleResult
{
    /// <summary>
    /// Gets or sets whether the operation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets a message about the operation
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
