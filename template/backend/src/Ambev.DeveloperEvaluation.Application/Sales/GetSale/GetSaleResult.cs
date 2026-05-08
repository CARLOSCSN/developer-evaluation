namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Result for GetSale operation
/// </summary>
public class GetSaleResult
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
    /// Gets or sets the items
    /// </summary>
    public List<GetSaleItemResult> Items { get; set; } = new();
}

/// <summary>
/// Result for a sale item
/// </summary>
public class GetSaleItemResult
{
    /// <summary>
    /// Gets or sets the item ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product name
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets or sets the total item amount
    /// </summary>
    public decimal TotalItemAmount { get; set; }

    /// <summary>
    /// Gets or sets whether this item is cancelled
    /// </summary>
    public bool Cancelled { get; set; }
}
