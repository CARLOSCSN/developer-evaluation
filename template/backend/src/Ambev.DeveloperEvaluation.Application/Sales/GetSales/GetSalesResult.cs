namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Result for GetSales operation
/// </summary>
public class GetSalesResult
{
    /// <summary>
    /// Gets or sets the list of sales
    /// </summary>
    public List<SaleDto> Sales { get; set; } = new();

    /// <summary>
    /// Gets or sets the total count of sales
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the current page
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the total pages
    /// </summary>
    public int TotalPages { get; set; }
}

/// <summary>
/// DTO for a sale in the list
/// </summary>
public class SaleDto
{
    /// <summary>
    /// Gets or sets the sale ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the sale number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date of the sale
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the customer ID
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the customer name
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch
    /// </summary>
    public string Branch { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total amount
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets whether the sale is cancelled
    /// </summary>
    public bool Cancelled { get; set; }

    /// <summary>
    /// Gets or sets the number of items
    /// </summary>
    public int ItemCount { get; set; }
}
